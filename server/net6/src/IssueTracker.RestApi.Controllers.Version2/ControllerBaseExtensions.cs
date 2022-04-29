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

using IssueTracker.Core.Model;
using IssueTracker.Core.Requests;
using IssueTracker.Data.Abstractions.Specifications;
using IssueTracker.RestApi.DataTransferObjects.Version2.QueryParameters;
using IssueTracker.RestApi.DataTransferObjects.Version2.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static IssueTracker.RestApi.Controllers.Shared.Validation.PagingValidation;
using SortDirection = IssueTracker.RestApi.DataTransferObjects.Version2.QueryParameters.SortDirection;

namespace IssueTracker.RestApi.Controllers.Version2;

internal static class ControllerBaseExtensions
{

    public static async Task<IActionResult> GetAllWithIssueType(
        this ControllerBase controller,
        IMediator mediator,
        IssueType type,
        int pageNumber = 1,
        int pageSize = 10,
        IssueSortBy orderBy = IssueSortBy.Title,
        SortDirection direction = SortDirection.Ascending,
        CancellationToken cancellationToken = default)
    {
        GetFilteredPagedAndSortedIssueSummariesRequest request = new(
            new WhereIssueTypeMatchesSingleValueSpecification(type),
            pageNumber,
            pageSize,
            orderBy.ToModel(),
            direction.ToModel());

        HashSet<IssueSummaryDto> summary = await IssueSummaryDto
            .MapFrom(await mediator.Send(request, cancellationToken), cancellationToken)
            .ToHashSetAsync(cancellationToken);

        return ValidatePaging(controller.ModelState, pageNumber, pageSize)
            ? controller.Ok(summary)
            : controller.BadRequest(controller.ModelState);
    }
}
