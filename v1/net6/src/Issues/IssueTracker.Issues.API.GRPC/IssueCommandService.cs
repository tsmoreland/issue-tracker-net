//
// Copyright (c) 2022 Terry Moreland
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

using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using IssueTracker.Issues.API.GRPC.Proto;
using IssueTracker.Issues.Domain.DataContracts;
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.Commands;
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.Exceptions;
using IssueTracker.Issues.Domain.Services.Version2.Commands;
using IssueTracker.Issues.Domain.Services.Version2.DataTransferObjects;
using MediatR;

namespace IssueTracker.Issues.API.GRPC;

public sealed class IssueCommandService : IssueTrackerCommandService.IssueTrackerCommandServiceBase
{
    private readonly IMediator _mediator;
    private readonly IIssueDataMigration _dataMigration;

    public IssueCommandService(IMediator mediator, IIssueDataMigration dataMigration)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _dataMigration = dataMigration ?? throw new ArgumentNullException(nameof(dataMigration));
    }

    /// <inheritdoc />
    public override async Task<Empty> ResetDatabase(Empty request, ServerCallContext context)
    {
        await _dataMigration.ResetAndRepopultateAsync(context.CancellationToken);
        return new Empty();
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

    /// <inheritdoc />
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

    /// <inheritdoc />
    public override async Task<StatusMessage> DeleteIssue(IssueCommandMessage request, ServerCallContext context)
    {
        return await _mediator.Send(new DeleteIssueCommand(IssueIdentifier.FromString(request.Id)))
            ? new StatusMessage { Status = ResultCode.ResultSuccess, Message = "" }
            : new StatusMessage { Status = ResultCode.ResultNotFound, Message = $"{request.Id} not found" };
    }

    private async Task<StatusMessage> ChangeState(IRequest<Unit> statusChangeCommand, ServerCallContext context)
    {
        try
        {
            await _mediator.Send(statusChangeCommand, context.CancellationToken);
            return new StatusMessage { Status = ResultCode.ResultSuccess, Message = "" };
        }
        catch (IssueNotFoundException ex)
        {
            return new StatusMessage { Status = ResultCode.ResultNotFound, Message = ex.Message };
        }
        catch (InvalidStateChangeException ex)
        {
            return new StatusMessage { Status = ResultCode.ResultInvalidArgument, Message = ex.Message };
        }

    }

    /// <inheritdoc />
    public override Task<StatusMessage> MoveToBackLogStateChange(IssueCommandMessage request, ServerCallContext context)
    {
        return ChangeState(new MoveToBackLogStateChangeCommand(IssueIdentifier.FromString(request.Id)), context);
    }

    /// <inheritdoc />
    public override Task<StatusMessage> OpenStateChange(IssueCommandMessage request, ServerCallContext context)
    {
        return ChangeState(new OpenStateChangeCommand(IssueIdentifier.FromString(request.Id), DateTimeOffset.UtcNow), context);
    }

    /// <inheritdoc />
    public override Task<StatusMessage> ReadyForReviewStateChange(IssueCommandMessage request, ServerCallContext context)
    {
        return ChangeState(new ReadyForReviewStateChangeCommand(IssueIdentifier.FromString(request.Id)), context);
    }

    /// <inheritdoc />
    public override Task<StatusMessage> ReadyForTestStateChange(IssueCommandMessage request, ServerCallContext context)
    {
        return ChangeState(new ReadyForTestStateChangeCommand(IssueIdentifier.FromString(request.Id)), context);
    }

    /// <inheritdoc />
    public override Task<StatusMessage> CompletedStateChange(IssueCommandMessage request, ServerCallContext context)
    {
        return ChangeState(new CompletedStateChangeCommand(IssueIdentifier.FromString(request.Id), DateTimeOffset.UtcNow), context);
    }

    /// <inheritdoc />
    public override Task<StatusMessage> CloseStateChange(IssueCommandMessage request, ServerCallContext context)
    {
        return ChangeState(new CloseStateChangeCommand(IssueIdentifier.FromString(request.Id)), context);
    }

    /// <inheritdoc />
    public override Task<StatusMessage> CannotReproduceStateChange(IssueCommandMessage request, ServerCallContext context)
    {
        return ChangeState(new CannotReproduceStateChangeCommand(IssueIdentifier.FromString(request.Id), DateTimeOffset.UtcNow), context);
    }

    /// <inheritdoc />
    public override Task<StatusMessage> WontDoStateChange(IssueCommandMessage request, ServerCallContext context)
    {
        return ChangeState(new WontDoStateChangeCommand(IssueIdentifier.FromString(request.Id), DateTimeOffset.UtcNow), context);
    }

    /// <inheritdoc />
    public override Task<StatusMessage> NotADefectStateChange(IssueCommandMessage request, ServerCallContext context)
    {
        return ChangeState(
            new NotADefectStateChangeCommand(
                IssueIdentifier.FromString(request.Id),
                DateTimeOffset.UtcNow),
            context);
    }

    /// <inheritdoc />
    public override Task<StatusMessage> ReviewFailedStateChange(IssueCommandMessage request, ServerCallContext context)
    {
        return ChangeState(new ReviewFailedStateChangeCommand(IssueIdentifier.FromString(request.Id)), context);
    }

    /// <inheritdoc />
    public override Task<StatusMessage> TestFailedStateChange(IssueCommandMessage request, ServerCallContext context)
    {
        return ChangeState(new TestFailedStateChangeCommand(IssueIdentifier.FromString(request.Id)), context);
    }
}
