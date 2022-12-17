//
// Copyright (c) 2022 Terry Moreland
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System.Text.Json;
using AutoMapper;
using IssueTracker.Issues.API.REST.Version2.Converters;
using IssueTracker.Issues.API.REST.Version2.DataTransferObjects.Request;
using IssueTracker.Issues.API.REST.Version2.DataTransferObjects.ResourceParameters;
using IssueTracker.Issues.API.REST.Version2.DataTransferObjects.Response;
using IssueTracker.Issues.API.REST.VersionIndependent.DataTransferObjects.Response;
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.Commands;
using IssueTracker.Issues.Domain.ModelAggregates.Specifications;
using IssueTracker.Issues.Domain.Services.Version2.Commands;
using IssueTracker.Issues.Domain.Services.Version2.Queries;
using IssueTracker.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace IssueTracker.Issues.API.REST.Version2.Controllers;

/// <summary>
/// Controller methods shared between <see cref="IssuesController"/> and <see cref="IssuesVersionHeaderOrQueryController"/>
/// </summary>
[ApiController]
public abstract class IssuesControllerBase : ControllerBase
{
    /// <summary/>
    protected IssuesControllerBase(IMediator mediator, IMapper mapper)
    {
        Mediator = mediator;
        Mapper = mapper;
    }

    /// <summary/>
    protected IMediator Mediator { get; }

    /// <summary/>
    protected IMapper Mapper { get; }

    /// <inheritdoc cref="IssuesController.GetPagedIssues"/>
    protected async Task<IActionResult> GetIssuesWithLinks(string routeName, IssuesResourceParameters issuesResourceParameters, bool includeLinks, CancellationToken cancellationToken = default)
    {
        (int pageNumber, int pageSize, string? orderBy, string?[]? priority, string? searchQuery) = issuesResourceParameters;
        PagingOptions paging = new(pageNumber, pageSize);
        SortingOptions sorting = SortingOptions.FromString(orderBy);
        FilterOptions filter = FilterOptions.FromNullableStringArray(priority);
        _ = filter; // TODO
        _ = searchQuery; // TODO
        GetAllSortedAndPagedSummaryQuery summaryQuery = new(paging, sorting);

        if (!paging.IsValid(out string? invalidProperty, out string? errorMessage))
        {
            ModelState.AddModelError(invalidProperty, errorMessage);
            return BadRequest(ModelState);
        }

        IssueSummaryPage page = IssueSummaryPage.Convert(await Mediator.Send(summaryQuery, cancellationToken), Mapper);

        // TODO: move this calculation into a property of Page (and the domain Page, dto Page should just get the value)
        // TODO: add total pages too that'll simplify these calculations
        string? previousPageLink = issuesResourceParameters.PageNumber > 1
            ? CreateIssuesResourceUri(routeName, issuesResourceParameters, ResourceUriType.PreviousPage)
            : null;
        string? nextPageLink = (issuesResourceParameters.PageNumber * issuesResourceParameters.PageSize) <= page.TotalCount
            ? CreateIssuesResourceUri(routeName, issuesResourceParameters, ResourceUriType.NextPage)
            : null;

        var paginationMetaData = new
        {
            totalCount = page.TotalCount,
            pageSize = issuesResourceParameters.PageSize,
            currentPage = page.PageNumber,
            totalPages = page.TotalPages,
            previousPageLink,
            nextPageLink,
        };


        JsonSerializerOptions jsonSerializerOptions = HttpContext.RequestServices.GetRequiredService<IOptions<JsonOptions>>()
            .Value
            .JsonSerializerOptions;
        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetaData, paginationMetaData.GetType(), jsonSerializerOptions));

        if (!includeLinks)
        {
            return Ok(page);
        }

        IssueSummaryPageWithLinks value = new(page,
            GetLinksForIssueCollection(issuesResourceParameters, previousPageLink, nextPageLink));

        return Ok(value);

    }

    /// <inheritdoc cref="IssuesController.Get(string, CancellationToken)"/>
    protected async Task<IActionResult> GetIssueById(IssueIdentifier id, bool includeLinks, CancellationToken cancellationToken)
    {
        IssueDto? issue = Mapper.Map<IssueDto?>(await Mediator
            .Send(new FindIssueDtoByIdQuery(id), cancellationToken));

        if (issue is null)
        {
            return NotFound();
        }

        return includeLinks
            ? Ok(new IssueDtoWithLinks(issue, GetLinksForIssue(issue.Id)))
            : Ok(issue);
    }

    /// <inheritdoc cref="IssuesController.Post(AddIssueDto, CancellationToken)"/>
    protected async Task<IActionResult> Create(string routeName, AddIssueDto model, bool includeLinks, CancellationToken cancellationToken)
    {
        (string project, string title, string description, Priority priority, IssueType type, string? epicId) = model;

        IssueDto issue = Mapper.Map<IssueDto>(await Mediator
            .Send(new CreateIssueCommand(
                project,
                title, description,
                priority, type,
                IssueIdentifier.FromStringIfNotNull(epicId)),
                cancellationToken));

        if (!includeLinks)
        {
            return CreatedAtRoute(routeName, new { id = issue.Id }, issue);
        }

        IssueDtoWithLinks value = new(issue, GetLinksForIssue(issue.Id));
        return CreatedAtRoute(routeName, new { id = issue.Id }, value);

    }

    /// <inheritdoc cref="IssuesController.Put(string, EditIssueDto, CancellationToken)"/>
    protected async Task<IActionResult> UpdateIssue(IssueIdentifier id, [FromBody] EditIssueDto model, CancellationToken cancellationToken)
    {
        (string? title, string? description, Priority? priority, IssueType type, string? epicId) = model;
        IssueDto? issue = Mapper.Map<IssueDto>(await Mediator.Send(
            new ModifyIssueCommand(id,
                title, description,
                priority, type,
                IssueIdentifier.FromStringIfNotNull(epicId)),
            cancellationToken));
        return issue is not null
            ? NoContent()
            : NotFound();
    }

    /// <inheritdoc cref="IssuesController.Patch(string, JsonPatchDocument{IssuePatch}?, CancellationToken)"/>
    protected async Task<IActionResult> PatchIssue(IssueIdentifier id, [FromBody] JsonPatchDocument<IssuePatch> patchDoc, CancellationToken cancellationToken)
    {
        Issue? issue = await Mediator.Send(new FindIssueByIdQuery(id, true), cancellationToken);
        if (issue is null)
        {
            return NotFound();
        }

        IssuePatch patch = IssuePatch.FromIssue(issue)!;

        patchDoc.ApplyTo(patch,
            error =>
            {
                ModelState
                    .AddModelError(
                        $"{error.Operation.op}:{error.Operation.path}",
                        error.ErrorMessage);
            });

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        IssueDto? dto = Mapper.Map<IssueDto?>((await Mediator
            .Send(new PatchIssueCommand(id, patch.GetChanges()), cancellationToken)));

        return dto != null
            ? NoContent()
            : NotFound();
    }

    /// <inheritdoc cref="IssuesController.Delete(string, CancellationToken)"/>
    protected async Task<IActionResult> DeleteIssue(IssueIdentifier id, CancellationToken cancellationToken)
    {
        return await Mediator.Send(new DeleteIssueCommand(id), cancellationToken)
            ? new StatusCodeResult(StatusCodes.Status204NoContent)
            : NotFound();
    }

    /// <inheritdoc cref="IssuesController.ChangeState(string, ChangeIssueStateDto, CancellationToken)"/>
    protected Task<IActionResult> ChangeIssueState(IssueIdentifier id, ChangeIssueStateDto changeState, CancellationToken cancellationToken)
    {
        if (ChangeIssueStateDtoConverter.TryConvertToCommand(id, changeState.State, out StateChangeCommand? command))
        {
            return Mediator.Send(command, cancellationToken)
                .ContinueWith<IActionResult>(_ => Ok(), cancellationToken);
        }

        ModelState.AddModelError("State", "unsupported state");
        return Task.FromResult<IActionResult>(BadRequest(new ValidationProblemDetails(ModelState)));
    }

    /// <inheritdoc />
    /// <remarks>
    /// Without this Validation Problems would ignore any customization done by ConfigureApiOptions as that only applies to
    /// Model Binding, so in this case using the InvalidModelStateResponseFactory ensures we get the 422 response as configured there.
    /// </remarks>
    public override ActionResult ValidationProblem([ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
    {
        IOptions<ApiBehaviorOptions> options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();
        return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
    }

    /// <summary>
    /// Returns HATEOAS links to be included in response
    /// </summary>
    /// <param name="issueId">id of the issue to display links for</param>
    /// <returns>Collection of <see cref="LinkDto"/></returns>
    protected abstract IEnumerable<LinkDto> GetLinksForIssue(string issueId);

    /// <summary>
    /// Returns HATEOAS links to be included in response
    /// </summary>
    /// <returns>Collection of <see cref="LinkDto"/></returns>
    protected abstract IEnumerable<LinkDto> GetLinksForIssueCollection(
        IssuesResourceParameters issuesResourceParameters, string? previousPageLink, string? nextPageLink);

    /// <summary>
    /// Returns Resource URI for current, next or previous page with provided query parameters
    /// </summary>
    /// <param name="routeName">route to navigate to</param>
    /// <param name="issuesResourceParameters">parameters used in that route</param>
    /// <param name="uriType"><see cref="ResourceUriType"/></param>
    /// <returns>link to current, previous or next page</returns>
    protected string? CreateIssuesResourceUri(string routeName, IssuesResourceParameters issuesResourceParameters, ResourceUriType uriType)
    {
        return uriType switch
        {
            ResourceUriType.PreviousPage => Url.Link(routeName, issuesResourceParameters.ToRouteParameters(issuesResourceParameters.PageNumber - 1)),
            ResourceUriType.NextPage => Url.Link(routeName, issuesResourceParameters.ToRouteParameters(issuesResourceParameters.PageNumber + 1)),
            ResourceUriType.Current => Url.Link(routeName, issuesResourceParameters.ToRouteParameters(issuesResourceParameters.PageNumber)),
            _ => Url.Link(routeName, issuesResourceParameters.ToRouteParameters(issuesResourceParameters.PageNumber + 1)),
        };
    }

}
