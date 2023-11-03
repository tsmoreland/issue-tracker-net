﻿//
// Copyright (c) 2023 Terry Moreland
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

using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.Projections;
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.Specifications;
using IssueTracker.Issues.Domain.ModelAggregates.Specifications;
using IssueTracker.Issues.Domain.Services.Version1.DataTransferObjects;
using IssueTracker.Issues.Domain.Services.Version1.Extensions;
using IssueTracker.Issues.Domain.Services.Version1.Queries;
using MediatR;

namespace IssueTracker.Issues.Domain.Services.Version1.QueryHandlers;

public sealed class GetAllSortedAndPagedSummaryQueryHandler : IRequestHandler<GetAllSortedAndPagedSummaryQuery, IssueSummaryPage>
{
    private readonly IIssueRepository _repository;

    public GetAllSortedAndPagedSummaryQueryHandler(IIssueRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }


    /// <inheritdoc />
    public async Task<IssueSummaryPage> Handle(GetAllSortedAndPagedSummaryQuery request, CancellationToken cancellationToken)
    {
        (PagingOptions paging, SortingOptions sorting) = request;
        (int totalPages, int totalCount, IAsyncEnumerable<IssueSummaryProjection> summaries) = await _repository
            .GetPagedAndSortedProjections(
                IPredicateSpecification<Issue>.None,
                new SelectIssueSummaryProjection(),
                paging, sorting,
                cancellationToken);

        return new IssueSummaryPage(paging.PageNumber, totalPages, totalCount, summaries.ToDto());
    }
}
