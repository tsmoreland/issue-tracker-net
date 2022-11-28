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
using IssueTracker.Issues.API.REST.Version2.DataTransferObjects.Response;
using IssueTracker.Issues.API.REST.Version2.Extensions;
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IssueTracker.Issues.API.REST.Version2.Controllers;

/// <summary>
/// Defects controller (v2)
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/defects")]
[Tags("Defects (URL versioning)")]
[ApiVersion("2")]
public sealed class DefectsController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Instantiates a new instance of the <see cref="TasksController"/> class.
    /// </summary>
    public DefectsController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// returns a summary of all issues with type of Defect
    /// </summary>
    /// <param name="pageNumber" example="1" >current page number to return</param>
    /// <param name="pageSize" example="10">maximum number of items to return</param>
    /// <param name="orderBy" example="Priority, EpicId, Title DESC" >order by spec</param>
    /// <param name="cancellationToken">a cancellation token.</param>
    /// <returns>all issues with type <see cref="Task"/></returns>
    [HttpGet]
    [HttpHead]
    [Consumes(MediaTypeNames.Application.Json, "text/json", "application/*+json", MediaTypeNames.Application.Xml)]
    [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [SwaggerResponse(StatusCodes.Status200OK, "Successful Response", typeof(IAsyncEnumerable<IssueSummaryDto>),
        MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid arguments", typeof(ProblemDetails),
        "application/problem+json", "application/problem+xml")]
    [Filters.ValidateModelStateServiceFilter]
    public Task<ActionResult<IAsyncEnumerable<IssueSummaryDto>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? orderBy = null,
        CancellationToken cancellationToken = default)
    {
        // extension method so 'this' is required
        return this.GetAllWithIssueType(
            _mediator,
            IssueType.Defect,
            pageNumber,
            pageSize,
            orderBy,
            cancellationToken);
    }
}
