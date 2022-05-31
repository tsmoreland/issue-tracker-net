using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;
using IssueTracker.Issues.Domain.Services.Version2.Commands.StateChangeCommands;
using MediatR;

namespace IssueTracker.Issues.Domain.Services.Version2.CommandHandlers.StateChangeCommandHandlers;

public sealed class ExecuteCloseStateChangeCommandHandler : IRequestHandler<ExecuteCloseStateChangeCommand, Unit>
{
    private readonly IIssueRepository _repository;

    public ExecuteCloseStateChangeCommandHandler(IIssueRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    /// <inheritdoc />
    public Task<Unit> Handle(ExecuteCloseStateChangeCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
