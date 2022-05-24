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

using Grpc.Core;
using IssueTracker.Issues.API.GRPC.Proto;
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;
using IssueTracker.Issues.Domain.Services.Version2.Commands;
using IssueTracker.Issues.Domain.Services.Version2.DataTransferObjects;
using MediatR;

namespace IssueTracker.Issues.API.GRPC;

public sealed class IssueCommandService : IssueTrackerCommandService.IssueTrackerCommandServiceBase
{
    private readonly IMediator _mediator;

    public IssueCommandService(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <inheritdoc />
    public override async Task<StatusMessage> AddIssue(AddIssueMessage request, ServerCallContext context)
    {
        IssueDto dto = await _mediator
            .Send(new CreateIssueCommand(
                    request.Project,
                    request.Title, request.Description,
                    request.Priority.ToDomainOrDefault(), request.Type.ToDomainOrDefault(),
                    IssueIdentifier.FromStringIfNotNull(request.EpicId.HasValue ? request.EpicId.Value : null)),
                context.CancellationToken);

        return new StatusMessage { Status = ResultCode.ResultSuccess, Message = dto.Id };
    }

    public override async Task<StatusMessage> EditIssue(EditIssueMessage request, ServerCallContext context)
    {
        IssueDto? dto = await _mediator.Send(
            new ModifyIssueCommand(IssueIdentifier.FromString(request.Id),
                request.Title, request.Description,
                request.Priority.ToDomainOrNull(), request.Type.ToDomainOrNull(),
                IssueIdentifier.FromStringIfNotNull(request.EpicId.HasValue ? request.EpicId.Value : null)),
            context.CancellationToken);
        return dto is not null
            ? new StatusMessage { Status = ResultCode.ResultSuccess, Message = "" }
            : new StatusMessage { Status = ResultCode.ResultNotFound, Message = "" };
    }
}
