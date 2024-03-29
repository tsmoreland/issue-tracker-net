﻿//
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InstallerTracker.Swashbuckle.Extensions;
using IssueTracker.Core.Model;
using IssueTracker.Core.Projections;
using IssueTracker.Core.Requests;
using IssueTracker.RestApi.Shared.Model.Request;
using IssueTracker.RestApi.Shared.Model.Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IssueTracker.NetCoreApp21.RestApi.App.Controllers;

[Route("api/v{version:apiVersion}/issuesV1")] // temporarily change endpoint due to swashbuckle problem - can't handle 2 routes with same path
[ApiController]
[TrimVersionFromSwagger]
[ApiVersion("1")]
public class IssuesV1Controller : ControllerBase
{
    private readonly IMediator _mediator;

    public IssuesV1Controller(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet]
    public async Task<IReadOnlyList<IssueSummaryV1>> GetIssueSummaries(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] IssueSortBy sortBy = IssueSortBy.Title,
        [FromQuery] SortDir direction = SortDir.Ascending,
        CancellationToken cancellationToken = default)
    {

        IReadOnlyList<IssueSummaryProjection> summaries = await _mediator
            .Send(new GetPagedAndSortedIssueSummariesRequest(pageNumber, pageSize, sortBy.ToSortBy(), direction.ToSortDirection()),
            cancellationToken);

        if (summaries.Count <= 0)
        {
            Response.StatusCode = StatusCodes.Status204NoContent;
        }
        else
        {
            Response.StatusCode = StatusCodes.Status200OK;
        }

        return summaries.Select(IssueSummaryV1.FromProjection).ToList();
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
    public IssueV1 GetIssueById([FromRoute] int id)
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


        return IssueV1.From(issue);
    }

    /// <remarks>
    /// continuing bad practice to demonstrate older style APIs (non-async/await)
    /// while still using mediation to handle the request
    /// </remarks>
    [HttpPost]
    public IssueV1 AddIssue([FromBody] AddIssueV1 model)
    {
        Issue issue = _mediator.Send(new CreateIssueRequest(model.ToModel()), CancellationToken.None).Result;

        if (issue is not null)
        {
            Response.StatusCode = StatusCodes.Status201Created;
            return IssueV1.From(issue);
        }
        else
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            return null;
        }
    }
}
