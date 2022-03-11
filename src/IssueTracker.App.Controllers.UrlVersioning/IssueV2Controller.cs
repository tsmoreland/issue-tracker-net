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
using IssueTracker.Services.Abstractions.Model.Response;
using IssueTracker.Services.Abstractions.Requests;
using IssueTracker.SwashbuckleExtensions.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static IssueTracker.App.Controllers.Shared.Validation.PagingValidation;

namespace IssueTracker.App.Controllers.UrlVersioning;

/// <summary>
/// Issues Controller (v2)
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/issues")]
[TrimVersionFromSwagger]
[ApiVersion("2")]
public sealed class IssueV2Controller : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Instantiates a new instance of the <see cref="IssueV2Controller"/> class.
    /// </summary>
    public IssueV2Controller(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Returns all parent issues 
    /// </summary>
    /// <param name="id" example="1385056E-8AFA-4E09-96DF-AE12EFDF1A29">unique id of issue</param>
    /// <param name="pageNumber" example="1" >current page number to return</param>
    /// <param name="pageSize" example="10">maximum number of items to return</param>
    /// <param name="cancellationToken">a cancellation token.</param>
    /// <returns>all parent issues</returns>
    [HttpGet("{id}/parents")]
    [Consumes(MediaTypeNames.Application.Json, "text/json", "application/*+json", "application/xml")]
    [Produces(MediaTypeNames.Application.Json, "application/xml")]
    [SwaggerResponse(StatusCodes.Status200OK, "Successful Response", typeof(IAsyncEnumerable<IssueSummaryDto>), MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid arguments", typeof(IAsyncEnumerable<IssueSummaryDto>), MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Issue not found", typeof(IAsyncEnumerable<IssueSummaryDto>), MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    public async Task<IActionResult> GetParentIssues(
        [FromRoute] Guid id,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        if (!ValidatePaging(ModelState, pageNumber, pageSize))
        {
            return BadRequest(ModelState);
        }

        if (await _mediator.Send(new IssueExistsRequest(id), cancellationToken))
        {
            return Ok(await _mediator.Send(new GetParentIssuesRequest(id, pageNumber, pageSize), cancellationToken));
        }

        ModelState.AddModelError(nameof(id), "issue not found");
        return NotFound(ModelState);
    }

    /// <summary>
    /// Returns all child issues 
    /// </summary>
    /// <param name="id" example="1385056E-8AFA-4E09-96DF-AE12EFDF1A29">unique id of issue</param>
    /// <param name="pageNumber" example="1" >current page number to return</param>
    /// <param name="pageSize" example="10">maximum number of items to return</param>
    /// <param name="cancellationToken">a cancellation token.</param>
    /// <returns>all child issues</returns>
    [HttpGet("{id}/children")]
    [Consumes(MediaTypeNames.Application.Json, "text/json", "application/*+json", "application/xml")]
    [Produces(MediaTypeNames.Application.Json, "application/xml")]
    [SwaggerResponse(StatusCodes.Status200OK, "Successful Response", typeof(IAsyncEnumerable<IssueSummaryDto>), MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid arguments", typeof(IAsyncEnumerable<IssueSummaryDto>), MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Issue not found", typeof(IAsyncEnumerable<IssueSummaryDto>), MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [ApiVersion("2")]
    public async Task<IActionResult> GetChildIssues(
        [FromRoute] Guid id,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        if (!ValidatePaging(ModelState, pageNumber, pageSize))
        {
            return BadRequest(ModelState);
        }

        if (await _mediator.Send(new IssueExistsRequest(id), cancellationToken))
        {
            return Ok(await _mediator.Send(new GetChildIssuesRequest(id, pageNumber, pageSize), cancellationToken));
        }

        ModelState.AddModelError(nameof(id), "issue not found");
        return NotFound(ModelState);
    }
}
