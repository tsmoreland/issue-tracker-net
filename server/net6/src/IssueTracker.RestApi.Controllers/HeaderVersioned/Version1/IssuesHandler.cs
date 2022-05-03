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

using IssueTracker.Core.Requests;
using IssueTracker.RestApi.DataTransferObjects.Version1.QueryParameters;
using IssueTracker.RestApi.DataTransferObjects.Version1.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using static IssueTracker.RestApi.Controllers.Shared.Validation.PagingValidation;
using SortDirection = IssueTracker.RestApi.DataTransferObjects.Version1.QueryParameters.SortDirection;

namespace IssueTracker.RestApi.Controllers.HeaderVersioned.Version1;

internal sealed class IssuesHandler
{
    private readonly IMediator _mediator;
    private readonly ControllerBase _controller;

    public IssuesHandler(IMediator mediator, ControllerBase controller)
    {
        _mediator = mediator;
        _controller = controller;
    }

    private ModelStateDictionary ModelState => _controller.ModelState;
    private OkObjectResult Ok(object? value) => _controller.Ok(value);
    private BadRequestObjectResult BadRequest(object? value) => _controller.BadRequest(value);

    public async Task<IActionResult> GetAll(
        int pageNumber = 1,
        int pageSize = 10,
        int orderByValue = (int)IssueSortBy.Title,
        int directionValue = (int)SortDirection.Ascending,
        CancellationToken cancellationToken = default)
    {
        if (!Enum.IsDefined(typeof(IssueSortBy), orderByValue))
        {
            ModelState.AddModelError("orderBy", "invalid order by value");
            return BadRequest(ModelState);
        }

        if (!Enum.IsDefined(typeof(SortDirection), directionValue))
        {
            ModelState.AddModelError("orderBy", "invalid sort direction");
            return BadRequest(ModelState);
        }

        IssueSortBy orderBy = (IssueSortBy)orderByValue;
        SortDirection direction = (SortDirection)directionValue;


        GetPagedAndSortedIssueSummariesRequest request = new(pageNumber, pageSize, orderBy.ToModel(),
            direction.ToModel());

        HashSet<IssueSummaryDto> summary = await IssueSummaryDto
            .MapFrom(await _mediator.Send(request, cancellationToken), cancellationToken)
            .ToHashSetAsync(cancellationToken);

        return ValidatePaging(ModelState, pageNumber, pageSize)
            ? Ok(summary)
            : BadRequest(ModelState);
    }
}
