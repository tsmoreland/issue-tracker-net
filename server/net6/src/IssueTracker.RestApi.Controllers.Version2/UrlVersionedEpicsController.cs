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
using IssueTracker.Core.Model;
using IssueTracker.Core.ValueObjects;
using IssueTracker.RestApi.DataTransferObjects.Version2.QueryParameters;
using IssueTracker.RestApi.DataTransferObjects.Version2.Response;
using IssueTracker.SwashbuckleExtensions.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using SortDirection = IssueTracker.RestApi.DataTransferObjects.Version2.QueryParameters.SortDirection;

namespace IssueTracker.RestApi.Controllers.Version2;

/// <summary>
/// Epics controller (v2)
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion=2}/epics")]
[TrimVersionFromSwagger]
[Tags("Epics")]
[ApiVersion("2")]
public sealed class UrlVersionedEpicsController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Instantiates a new instance of the <see cref="UrlVersionedEpicsController"/> class.
    /// </summary>
    public UrlVersionedEpicsController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// returns a summary of all issues with type of Epic
    /// </summary>
    /// <param name="pageNumber" example="1" >current page number to return</param>
    /// <param name="pageSize" example="10">maximum number of items to return</param>
    /// <param name="orderBy">property to order results by</param>
    /// <param name="direction">direction to order results by</param>
    /// <param name="cancellationToken">a cancellation token.</param>
    /// <returns>all issues with type <see cref="IssueType.Epic"/></returns>
    [HttpGet]
    [Consumes(MediaTypeNames.Application.Json, "text/json", "application/*+json", MediaTypeNames.Application.Xml)]
    [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [SwaggerResponse(StatusCodes.Status200OK, "Successful Response", typeof(IAsyncEnumerable<IssueSummaryDto>),
        MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid arguments", typeof(ProblemDetails),
        "application/problem+json", "application/problem+xml")]
    public Task<IActionResult> GetAllEpics(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] IssueSortBy orderBy = IssueSortBy.Title,
        [FromQuery] SortDirection direction = SortDirection.Ascending,
        CancellationToken cancellationToken = default)
    {
        // extension method so 'this' is required
        return this.GetAllWithIssueType(
            _mediator,
            IssueType.Epic,
            pageNumber,
            pageSize,
            orderBy,
            direction,
            cancellationToken);
    }
}
