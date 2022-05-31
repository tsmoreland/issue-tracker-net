using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;
using IssueTracker.Issues.Domain.Services.Version2.Commands.StateChangeCommands;
using MediatR;

namespace IssueTracker.Issues.Domain.Services.Version2.CommandHandlers.StateChangeCommandHandlers;

public sealed class ExecuteReviewFailedStateChangeCommandHandler : IRequestHandler<ExecuteReviewFailedStateChangeCommand, Unit>
{
    private readonly IIssueRepository _repository;

    public ExecuteReviewFailedStateChangeCommandHandler(IIssueRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    /// <inheritdoc />
    public Task<Unit> Handle(ExecuteReviewFailedStateChangeCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
