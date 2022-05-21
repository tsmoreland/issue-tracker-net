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

using AutoMapper;
using IssueTracker.Issues.API.Version2.REST.DataTransferObjects.Request;
using IssueTracker.Issues.API.Version2.REST.DataTransferObjects.Response;
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;
using IssueTracker.Issues.Domain.ModelAggregates.Specifications;
using IssueTracker.Issues.Domain.Services.Version2.Commands;
using IssueTracker.Issues.Domain.Services.Version2.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IssueTracker.Issues.API.Version2.REST.Extensions;

internal static class ControllerBaseExtensions
{

    public static async Task<IActionResult> GetAllWithIssueType(
        this ControllerBase controller,
        IMediator mediator,
        IssueType type,
        int pageNumber,
        int pageSize,
        [FromQuery] string? orderBy = null,
        CancellationToken cancellationToken = default)
    {
        PagingOptions paging = new (pageNumber, pageSize);
        SortingOptions sorting = SortingOptions.FromString(orderBy);
        GetAllSortedAndPagedWhereIssueTypeMatchesQuery query = new(paging, sorting, type);

        if (paging.IsValid(out string? invalidProperty, out string? errorMessage))
        {
            return controller.Ok(await mediator.Send(query, cancellationToken));
        }

        controller.ModelState.AddModelError(invalidProperty, errorMessage);
        return controller.BadRequest(controller.ModelState);
    }

    public static async Task<IActionResult> Post(
        this ControllerBase controller,
        IMediator mediator,
        IMapper mapper,
        IssueType type,
        string? epicId,
        AddIssueDtoBase model,
        CancellationToken cancellationToken)
    {
        (string project, string title, string description, Priority priority) = model;

        IssueDto issue = mapper.Map<IssueDto>(await mediator
            .Send(new CreateIssueCommand(
                project,
                title, description,
                priority, type,
                IssueIdentifier.FromStringIfNotNull(epicId)),
                cancellationToken));
        return controller.Created($"/api/v2/issues/{issue.Id}", issue);
    }
}
