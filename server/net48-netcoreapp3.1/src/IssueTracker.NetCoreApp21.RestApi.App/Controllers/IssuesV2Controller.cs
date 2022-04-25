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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IssueTracker.Core.Model;
using IssueTracker.Core.Projections;
using IssueTracker.Core.Requests;
using IssueTracker.NetCoreApp21.RestApi.App.Model.Request;
using IssueTracker.NetCoreApp21.RestApi.App.Model.Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IssueTracker.NetCoreApp21.RestApi.App.Controllers;

[ApiVersion("2")]
[Route("api/v{version:apiVersion}/issues")]
[ApiController]
public sealed class IssuesV2Controller : ControllerBase
{
    private readonly IMediator _mediator;

    public IssuesV2Controller(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet]
    public async Task<IReadOnlyList<IssueSummaryV2>> GetIssueSummaries(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] IssueSortBy sortBy = IssueSortBy.Title,
        [FromQuery] SortDir direction = SortDir.Ascending,
        CancellationToken cancellationToken = default)
    {

        IReadOnlyList<IssueSummaryProjection> summaries = await _mediator
            .Send(new GetPagedAndSortedIssueSummariesRequest(pageNumber, pageSize, sortBy.ToSortBy(), direction.ToSortDirection()),
            cancellationToken);

        Response.StatusCode = summaries.Count <= 0
            ? StatusCodes.Status204NoContent
            : StatusCodes.Status200OK;

        return summaries.Select(IssueSummaryV2.FromProjection).ToList();
    }

    /// <summary>
    /// Get issue by id
    /// </summary>
    /// <param name="id">id of issue to get</param>
    /// <returns>the matching issue or <see langword="null"/> if not found</returns>
    /// <remarks>
    /// intentionally misuing the <see langword="async"/>/<see langword="await"/> pattern
    /// to expocse return type without Task
    /// </remarks>
    [HttpGet("{id}")]
    public IssueV2 GetIssueById([FromRoute] int id)
    {
        Issue issue = _mediator.Send(new FindIssueByIdRequest(id), CancellationToken.None).Result;
        if (issue is null)
        {
            Response.StatusCode = StatusCodes.Status404NotFound;
            return null;
        }
        else
        {
            Response.StatusCode = StatusCodes.Status200OK;
        }


        return IssueV2.From(issue);
    }
}
