//
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

using Grpc.Core;
using IssueTracker.Issues.API.GRPC.Proto;
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;
using IssueTracker.Issues.Domain.ModelAggregates.Specifications;
using IssueTracker.Issues.Domain.Services.Version2.DataTransferObjects;
using IssueTracker.Issues.Domain.Services.Version2.Queries;
using MediatR;

namespace IssueTracker.Issues.API.GRPC;

public sealed class IssueQueryService : IssueTrackerQueryService.IssueTrackerQueryServiceBase
{
    private readonly IMediator _mediator;

    public IssueQueryService(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }


    /// <inheritdoc />
    public override async Task<IssueMessage> GetIssueById(IssueByIdQueryMessage request, ServerCallContext context)
    {
        IssueDto? issue = await _mediator
            .Send(new FindIssueDtoByIdQuery(IssueIdentifier.FromString(request.Id)), context.CancellationToken);

        return issue is not null
            ? issue.ToMessage()
            : IssueMessageFunctions.NotFound();
    }

    /// <inheritdoc />
    public override async Task<IssueSummariesMessage> GetIssues(PagedIssueRequestMessage request, ServerCallContext context)
    {
        if (!request.IsValid(out IssueSummariesMessage? error))
        {
            return error;
        }

        IssueSummaryPage page = await _mediator.Send(request.ToMediatorRequest(), context.CancellationToken);


        IssueSummariesMessage summaries = new() { PageNumber = page.PageNumber, Total = page.TotalCount };
        await foreach (IssueSummaryDto summary in page.Items)
        {
            summaries.Summaries.Add(summary.ToMessage());
        }

        summaries.Status = ResultCode.ResultSuccess;
        return summaries;
    }

    /// <inheritdoc />
    public override async Task GetAllIssues(IssueStreamRequestMessage request, IServerStreamWriter<IssueSummaryMessage> responseStream, ServerCallContext context)
    {
        IssueSummaryPage page = await _mediator.Send(new GetAllSortedAndPagedSummaryQuery(
            new PagingOptions(1, int.MaxValue),
            SortingOptions.FromString(request.OrderBy)), context.CancellationToken);

        Task lastWrite = Task.CompletedTask;
        await foreach (IssueSummaryDto summary in page.Items)
        {
            await lastWrite;
            if (context.CancellationToken.IsCancellationRequested)
            {
                return;
            }

            lastWrite = responseStream.WriteAsync(summary.ToMessage());
        }

        await base.GetAllIssues(request, responseStream, context);
    }

}
