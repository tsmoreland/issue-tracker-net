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

using System.Linq;
using Grpc.Core;
using IssueTracker.Core.Model;
using IssueTracker.Core.Projections;
using IssueTracker.Core.Requests;
using IssueTracker.GrpcApi.Grpc.Services;
using MediatR;
using static IssueTracker.GrpcApi.Grpc.Services.IssueTrackerService;

namespace IssueTracker.GrpcApi.Services;

public sealed class IssueService : IssueTrackerServiceBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// instantiates a new instance of the <see cref="IssueService"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// if <paramref name="mediator"/> is <see langword="null"/>
    /// </exception>
    public IssueService(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <inheritdoc/>
    public override async Task<IssueMessage> GetIssueById(IssueByIdQueryMessage request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.Id, out Guid id))
        {
            return IssueMessageFactory.InvalidArgument();
        }

        Issue? issue = await _mediator.Send(new FindIssueByIdRequest(id), context.CancellationToken);
        return issue is not null
            ? IssueMessageFactory.FromIssue(issue)
            : IssueMessageFactory.NotFound(); 
    }

    /// <inheritdoc/>
    public override async Task<IssueSummariesMessage> GetIssues(PagedRequestMessage request, ServerCallContext context)
    {
        if (!request.IsValid(out IssueSummariesMessage? error))
        {
            return error;
        }

        IAsyncEnumerable<IssueSummaryProjection> projections = await _mediator.Send(request.ToMediatorRequest(), context.CancellationToken);

        IssueSummariesMessage summariesMessage = new ();
        await foreach (IssueSummaryProjection projection in projections)
        {
            summariesMessage.Summaries.Add(projection.ToMessage());
        }

        summariesMessage.Status = ResultCode.Success;

        return summariesMessage;
    }
}
