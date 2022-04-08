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
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using IssueTracker.Core.Projections;
using IssueTracker.Core.Requests;
using IssueTracker.Data.Abstractions;
using MediatR;

namespace IssueTracker.Services.Handlers;

public sealed class GetAllIssuesRequestHandler : IRequestHandler<GetAllIssuesRequest, IAsyncEnumerable<IssueSummaryProjection>>
{
    private readonly IIssueRepository _repository;

    public GetAllIssuesRequestHandler(IIssueRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    /// <inheritdoc />
    public Task<IAsyncEnumerable<IssueSummaryProjection>> Handle(GetAllIssuesRequest request, CancellationToken cancellationToken)
    {
        (int pageNumber, int pageSize) = request;
        return Task.FromResult(HandleWithEnumerable(pageNumber, pageSize, cancellationToken));
    }

    private async IAsyncEnumerable<IssueSummaryProjection> HandleWithEnumerable(int pageNumber, int pageSize,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        ConfiguredCancelableAsyncEnumerable<IssueSummaryProjection> issues = _repository
            .GetIssueSummaries(pageNumber, pageSize, cancellationToken)
            .WithCancellation(cancellationToken);

        await foreach (IssueSummaryProjection projection in issues.WithCancellation(cancellationToken))
        {
            if (cancellationToken.IsCancellationRequested)
            {
                yield break;
            }

            yield return projection;
        }
    }
}
