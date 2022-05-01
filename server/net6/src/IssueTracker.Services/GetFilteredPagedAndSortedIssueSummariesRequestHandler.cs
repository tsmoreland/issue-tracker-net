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

using System.Runtime.CompilerServices;
using IssueTracker.Core.Model;
using IssueTracker.Core.Projections;
using IssueTracker.Core.Requests;
using IssueTracker.Core.Specifications;
using IssueTracker.Core.ValueObjects;
using IssueTracker.Data.Abstractions;
using MediatR;

namespace IssueTracker.Services;

public sealed class GetFilteredPagedAndSortedIssueSummariesRequestHandler : IRequestHandler<GetFilteredPagedAndSortedIssueSummariesRequest, IAsyncEnumerable<IssueSummaryProjection>>
{
    private readonly IIssueRepository _repository;

    public GetFilteredPagedAndSortedIssueSummariesRequestHandler(IIssueRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    /// <inheritdoc />
    public Task<IAsyncEnumerable<IssueSummaryProjection>> Handle(GetFilteredPagedAndSortedIssueSummariesRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(HandleWithEnumerable(request, cancellationToken));
    }

    private async IAsyncEnumerable<IssueSummaryProjection> HandleWithEnumerable(GetFilteredPagedAndSortedIssueSummariesRequest request,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        (WhereClauseSpecification specification, int pageNumber, int pageSize, Issue.SortBy sortBy, SortDirection direction) = request;

        ConfiguredCancelableAsyncEnumerable<IssueSummaryProjection> issues = _repository
            .GetFilteredIssueSummaries(new [] { specification }, pageNumber, pageSize, sortBy, direction, cancellationToken)
            .WithCancellation(cancellationToken);

        await foreach (IssueSummaryProjection projection in issues)
        {
            yield return projection;
        }
    }
}
