//
// Copyright © 2022 Terry Moreland
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
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using SortDirection = IssueTracker.RestApi.DataTransferObjects.Version2.QueryParameters.SortDirection;

using Version1Response = IssueTracker.RestApi.DataTransferObjects.Version1.Response;
using Version2QueryParameters = IssueTracker.RestApi.DataTransferObjects.Version2.QueryParameters;
using Version2Response = IssueTracker.RestApi.DataTransferObjects.Version2.Response;

namespace IssueTracker.RestApi.Controllers.HeaderVersioned;

/// <summary>
/// Issues Controller (v2)
/// </summary>
[ApiController]
[Route("api/issues")]
[Tags("Issues (Header versioning)")]
[ApiVersion("1")]
[ApiVersion("2")]
public sealed class IssueController : ControllerBase
{
    private readonly Version1.IssuesHandler _v1Handler;
    private readonly Version2.IssuesHandler _v2Handler;

    /// <summary>
    /// Instantiates a new instance of the <see cref="IssueController"/> class.
    /// </summary>
    public IssueController(IMediator mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator, nameof(mediator));

        _v1Handler = new Version1.IssuesHandler(mediator, this);
        _v2Handler = new Version2.IssuesHandler(mediator, this);
    }

    /// <summary>
    /// Returns all issues 
    /// </summary>
    /// <param name="version">API version</param>
    /// <param name="pageNumber" example="1" >current page number to return</param>
    /// <param name="pageSize" example="10">maximum number of items to return</param>
    /// <param name="orderBy">property to order results by</param>
    /// <param name="direction">direction to order results by</param>
    /// <param name="cancellationToken">a cancellation token.</param>
    /// <returns>all issues</returns>
    [HttpGet]
    [Consumes(MediaTypeNames.Application.Json, "text/json", "application/*+json", MediaTypeNames.Application.Xml)]
    [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [SwaggerResponse(StatusCodes.Status200OK, "Successful Response", typeof(IAsyncEnumerable<Version1Response.IssueSummaryDto>), MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [SwaggerResponse(StatusCodes.Status200OK, "Successful Response", typeof(IAsyncEnumerable<Version2Response.IssueSummaryDto>), MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid arguments", typeof(ProblemDetails), "application/problem+json", "application/problem+xml")]
    public Task<IActionResult> GetAll(
        [FromHeader] ApiVersion version,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] Version2QueryParameters.IssueSortBy orderBy = Version2QueryParameters.IssueSortBy.Title,
        [FromQuery] SortDirection direction = SortDirection.Ascending,
        CancellationToken cancellationToken = default)
    {
        return version switch
        {
            { MajorVersion: 1 } => _v1Handler.GetAll(pageNumber, pageSize, (int)orderBy, (int)direction, cancellationToken),
            { MajorVersion: 2 } => _v1Handler.GetAll(pageNumber, pageSize, (int)orderBy, (int)direction, cancellationToken),
            _ => InvalidVersion(),
        };

        Task<IActionResult> InvalidVersion()
        {
            ModelState.AddModelError("version", "unsupported version");
            return Task.FromResult<IActionResult>(BadRequest(ModelState));
        }
    }

    /*
    /// <summary>
    /// Returns issue matching <paramref name="id"/> if found
    /// </summary>
    /// <param name="id" example="APP-1234">unique id of issue</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns><see cref="IssueDto"/> matching <paramref name="id"/> if found</returns>
    [HttpGet("{id}")]
    [Consumes(MediaTypeNames.Application.Json, "text/json", "application/*+json", MediaTypeNames.Application.Xml)]
    [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [SwaggerResponse(StatusCodes.Status200OK, "Successful Response", typeof(IssueDto), MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid arguments", typeof(ProblemDetails), "application/problem+json", "application/problem+xml")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Issue not found", typeof(ProblemDetails), "application/problem+json", "application/problem+xml")]
    [ValidateIssueIdServiceFilter]
    public async Task<IActionResult> Get(string id, CancellationToken cancellationToken)
    {
        Version2Response.IssueDto? issue = IssueDto.From(await _mediator.Send(new FindIssueByIdRequest(IssueIdentifier.FromString(id)), cancellationToken));
        return issue is not null
            ? Ok(issue)
            : NotFound();
    }

    /// <summary>
    /// Returns all parent issues 
    /// </summary>
    /// <param name="id" example="APP-1234">unique id of issue</param>
    /// <param name="pageNumber" example="1" >current page number to return</param>
    /// <param name="pageSize" example="10">maximum number of items to return</param>
    /// <param name="cancellationToken">a cancellation token.</param>
    /// <returns>all parent issues</returns>
    [HttpGet("{id}/parents")]
    [Consumes(MediaTypeNames.Application.Json, "text/json", "application/*+json", MediaTypeNames.Application.Xml)]
    [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [SwaggerResponse(StatusCodes.Status200OK, "Successful Response", typeof(IAsyncEnumerable<LinkedIssueSummaryDto>), MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid arguments", typeof(ProblemDetails), "application/problem+json", "application/problem+xml")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Issue not found", typeof(ProblemDetails), "application/problem+json", "application/problem+xml")]
    [ValidateIssueIdServiceFilter]
    public async Task<IActionResult> GetParentIssues(
        [FromRoute] string id,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        if (!ValidatePaging(ModelState, pageNumber, pageSize))
        {
            return BadRequest(ModelState);
        }

        IssueIdentifier issueId = IssueIdentifier.FromString(id);
        if (await _mediator.Send(new IssueExistsRequest(issueId), cancellationToken))
        {
            return Ok(LinkedIssueSummaryDto
                .MapFrom(await _mediator
                    .Send(new GetParentIssueSummariesRequest(issueId, pageNumber, pageSize, Core.Model.Issue.SortBy.Title, Core.ValueObjects.SortDirection.Ascending)
                        , cancellationToken)
                    , cancellationToken));
        }

        ModelState.AddModelError(nameof(id), "issue not found");
        return NotFound(ModelState);
    }

    /// <summary>
    /// Returns all child issues 
    /// </summary>
    /// <param name="id" example="APP-1234">unique id of issue</param>
    /// <param name="pageNumber" example="1" >current page number to return</param>
    /// <param name="pageSize" example="10">maximum number of items to return</param>
    /// <param name="cancellationToken">a cancellation token.</param>
    /// <returns>all child issues</returns>
    [HttpGet("{id}/children")]
    [Consumes(MediaTypeNames.Application.Json, "text/json", "application/*+json", MediaTypeNames.Application.Xml)]
    [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [SwaggerResponse(StatusCodes.Status200OK, "Successful Response", typeof(IAsyncEnumerable<LinkedIssueSummaryDto>), MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid arguments", typeof(ProblemDetails), "application/problem+json", "application/problem+xml")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Issue not found", typeof(ProblemDetails), "application/problem+json", "application/problem+xml")]
    [ValidateIssueIdServiceFilter]
    public async Task<IActionResult> GetChildIssues(
        [FromRoute] string id,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        if (!ValidatePaging(ModelState, pageNumber, pageSize))
        {
            return BadRequest(ModelState);
        }

        IssueIdentifier issueId = IssueIdentifier.FromString(id);
        if (await _mediator.Send(new IssueExistsRequest(issueId), cancellationToken))
        {
            return Ok(LinkedIssueSummaryDto
                .MapFrom(await _mediator
                    .Send(new GetChildIssueSummariesRequest(issueId, pageNumber, pageSize, Core.Model.Issue.SortBy.Title, Core.ValueObjects.SortDirection.Ascending)
                        , cancellationToken)
                    , cancellationToken));
        }

        ModelState.AddModelError(nameof(id), "issue not found");
        return NotFound(ModelState);
    }

    /// <summary>
    /// Adds a new issue 
    /// </summary>
    /// <param name="model">the issue to add</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>newly created <see cref="IssueDto"/></returns>
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json, "text/json", "application/*+json", MediaTypeNames.Application.Xml)]
    [SwaggerResponse(StatusCodes.Status201Created, "Successful Response", typeof(IssueDto), MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid arguments", typeof(ProblemDetails), "application/problem+json", "application/problem+xml")]
    public async Task<IActionResult> Post([FromBody] AddIssueDto model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ValidationProblemDetails(ModelState));
        }

        IssueDto issue = IssueDto.From(await _mediator.Send(new CreateIssueRequest(model.ToModel()), cancellationToken));
        return new ObjectResult(issue) { StatusCode = StatusCodes.Status201Created };
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
    [SwaggerResponse(StatusCodes.Status200OK, "Successful Response", typeof(IssueDto), MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid arguments", typeof(ProblemDetails), "application/problem+json", "application/problem+xml")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Issue not found", typeof(ProblemDetails), "application/problem+json", "application/problem+xml")]
    [ValidateIssueIdServiceFilter]
    public async Task<IActionResult> Put(string id, [FromBody] EditIssueDto model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ValidationProblemDetails(ModelState));
        }

        IssueDto? issue = IssueDto.From(await _mediator.Send(new EditIssueRequest(IssueIdentifier.FromString(id), model.Update), cancellationToken));
        return issue is not null
            ? Ok(issue)
            : NotFound();
    }

    /// <summary>
    /// Deletes the issue given by <paramref name="id"/> 
    /// </summary>
    /// <param name="id" example="APP-1234">unique id of the issue to delete</param>
    /// <param name="cancellationToken">A cancellation token</param>
    [HttpDelete("{id}")]
    [Consumes(MediaTypeNames.Application.Json, "text/json", "application/*+json", MediaTypeNames.Application.Xml)]
    [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Successful Response", ContentTypes = new [] { MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml })]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid arguments", typeof(ProblemDetails), "application/problem+json", "application/problem+xml")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Issue not found", typeof(ProblemDetails), "application/problem+json", "application/problem+xml")]
    [ValidateIssueIdServiceFilter]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
    {
        return await _mediator.Send(new DeleteIssueRequest(IssueIdentifier.FromString(id)), cancellationToken)
            ? new StatusCodeResult(StatusCodes.Status204NoContent)
            : NotFound();
    }
    */
}
