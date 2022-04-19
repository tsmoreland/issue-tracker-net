﻿//
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
using IssueTracker.Core.Projections;
using IssueTracker.Core.Requests;
using IssueTracker.Data.Abstractions;
using MediatR;

namespace IssueTracker.Services;

public sealed class GetParentIssuesRequestHandler : IRequestHandler<GetParentIssuesRequest, IAsyncEnumerable<LinkedIssueSummaryProjection>> 
{
    private readonly IIssueRepository _repository;

    public GetParentIssuesRequestHandler(IIssueRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    /// <inheritdoc />
    public Task<IAsyncEnumerable<LinkedIssueSummaryProjection>> Handle(GetParentIssuesRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(HandleWithEnumerable(request, cancellationToken));
    }

    private async IAsyncEnumerable<LinkedIssueSummaryProjection> HandleWithEnumerable(GetParentIssuesRequest request,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        (Guid id, int pageNumber, int pageSize, _, _) = request;

        ConfiguredCancelableAsyncEnumerable<LinkedIssueSummaryProjection> issues = _repository
            .GetParentIssues(id, pageNumber, pageSize, cancellationToken)
            .WithCancellation(cancellationToken);

        await foreach (LinkedIssueSummaryProjection issueSummary in issues)
        {
            yield return issueSummary;
        }
    }
}
