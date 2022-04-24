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
using System.Threading;
using System.Threading.Tasks;
using IssueTracker.Core.Model;
using IssueTracker.Core.Projections;
using IssueTracker.Core.Requests;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IssueTracker.NetCoreApp21.RestApi.App.Controllers;

[Route("api/issues")]
[ApiController]
public class IssuesController : ControllerBase
{
    private readonly IMediator _mediator;

    public IssuesController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    // TODO: replace Model with DTOs - we shouldn't be using Core objects directly
    [HttpGet]
    public async Task<IReadOnlyList<IssueSummaryProjection>> GetIssueSummaries(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] Issue.SortBy sortBy = Issue.SortBy.Title,
        [FromQuery] SortDirection direction = SortDirection.Ascending,
        CancellationToken cancellationToken = default)
    {

        IReadOnlyList<IssueSummaryProjection> summaries = await _mediator.Send(new GetPagedAndSortedIssueSummariesRequest(pageNumber, pageSize, sortBy, direction),
            cancellationToken);

        if (summaries.Count <= 0)
        {
            Response.StatusCode = StatusCodes.Status204NoContent;
        }
        else
        {
            Response.StatusCode = StatusCodes.Status200OK;
        }

        return summaries;
    }

}
