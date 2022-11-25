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

using System.Net.Mime;
using AutoMapper;
using IssueTracker.Issues.API.REST.Version2.DataTransferObjects.Request;
using IssueTracker.Issues.API.REST.Version2.DataTransferObjects.Response;
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;
using IssueTracker.Issues.Domain.ModelAggregates.Specifications;
using IssueTracker.Issues.Domain.Services.Version2.Commands;
using IssueTracker.Issues.Domain.Services.Version2.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IssueTracker.Issues.API.REST.Version2.Controllers;

/// <summary>
/// Controller methods shared between <see cref="IssuesController"/> and <see cref="IssuesVersionHeaderOrQueryController"/>
/// </summary>
[ApiController]
public abstract class IssuesSharedControllerBase : IssuesControllerBase
{
    /// <summary/>
    protected IssuesSharedControllerBase(IMediator mediator, IMapper mapper)
        : base(mediator, mapper)
    {

    }

    /// <inheritdoc cref="IssuesController.GetAll(int, int, string?, CancellationToken)"/>
    protected async Task<ActionResult<IssueSummaryPage>> GetAllIssues(int pageNumber, int pageSize, string? orderBy, CancellationToken cancellationToken = default)
    {
        PagingOptions paging = new(pageNumber, pageSize);
        SortingOptions sorting = SortingOptions.FromString(orderBy);
        GetAllSortedAndPagedSummaryQuery summaryQuery = new(paging, sorting);

        if (paging.IsValid(out string? invalidProperty, out string? errorMessage))
        {
            return Ok(IssueSummaryPage.Convert(await Mediator.Send(summaryQuery, cancellationToken), Mapper));
        }

        ModelState.AddModelError(invalidProperty, errorMessage);
        return BadRequest(ModelState);
    }

    /// <inheritdoc cref="IssuesController.Get(string, CancellationToken)"/>
    protected async Task<ActionResult<IssueDto>> GetIssueById(string id, CancellationToken cancellationToken)
    {
        IssueDto? issue = Mapper.Map<IssueDto?>(await Mediator
            .Send(new FindIssueDtoByIdQuery(IssueIdentifier.FromString(id)), cancellationToken));
        return issue is not null
            ? Ok(issue)
            : NotFound();
    }

    /// <inheritdoc cref="IssuesController.Post(AddIssueDto, CancellationToken)"/>
    protected async Task<ActionResult<IssueDto>> Create(string routeName, AddIssueDto model, CancellationToken cancellationToken)
    {
        (string project, string title, string description, Priority priority, IssueType type, string? epicId) = model;

        IssueDto issue = Mapper.Map<IssueDto>(await Mediator
            .Send(new CreateIssueCommand(
                project,
                title, description,
                priority, type,
                IssueIdentifier.FromStringIfNotNull(epicId)),
                cancellationToken));

        return CreatedAtRoute(routeName, new { id = issue.Id }, issue);
    }

    /// <summary>
    /// Updates existing issue given by <paramref name="id"/>
    /// </summary>
    /// <param name="id">unique id of the issue to update</param>
    /// <param name="model">new values for the issue</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>updated <see cref="IssueDto"/> matching <paramref name="id"/> if found</returns>
    [HttpPut("{id}")]
    [Consumes(MediaTypeNames.Application.Json, "text/json", "application/*+json", MediaTypeNames.Application.Xml)]
    [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [SwaggerResponse(StatusCodes.Status200OK, "Successful Response")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid arguments", typeof(ProblemDetails), "application/problem+json", "application/problem+xml")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Issue not found", typeof(ProblemDetails), "application/problem+json", "application/problem+xml")]
    [Filters.ValidateIssueIdServiceFilter]
    [Filters.ValidateModelStateServiceFilter]
    public async Task<ActionResult<IssueDto>> Put(string id, [FromBody] EditIssueDto model, CancellationToken cancellationToken)
    {
        (string? title, string? description, Priority? priority, IssueType type, string? epicId) = model;
        IssueDto? issue = Mapper.Map<IssueDto>(await Mediator.Send(
            new ModifyIssueCommand(IssueIdentifier.FromString(id),
                title, description,
                priority, type,
                IssueIdentifier.FromStringIfNotNull(epicId)),
            cancellationToken));
        return issue is not null
            ? NoContent()
            : NotFound();
    }

    /// <summary>
    /// Updates existing issue given by <paramref name="id"/>
    /// </summary>
    /// <param name="id">unique id of the issue to update</param>
    /// <param name="patchDoc">patch document containing details on changes to </param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns></returns>
    [HttpPatch("{id}")]
    [Consumes("application/json-patch+json")]
    [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [SwaggerResponse(StatusCodes.Status200OK, "Successful Response")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid arguments", typeof(ProblemDetails), "application/problem+json", "application/problem+xml")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Issue not found", typeof(ProblemDetails), "application/problem+json", "application/problem+xml")]
    [Filters.ValidateModelStateServiceFilter]
    public async Task<ActionResult<IssueDto>> Patch(string id, [FromBody] JsonPatchDocument<IssuePatch>? patchDoc, CancellationToken cancellationToken)
    {
        IssueIdentifier issueId = IssueIdentifier.FromString(id);

        Issue? issue = await Mediator.Send(new FindIssueByIdQuery(issueId, true), cancellationToken);
        if (issue is null)
        {
            return NotFound();
        }

        if (patchDoc is null)
        {
            return BadRequest(ModelState);
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
            .Send(new PatchIssueCommand(issueId, patch.GetChanges()), cancellationToken)));

        return dto != null
            ? NoContent()
            : NotFound();
    }
}
