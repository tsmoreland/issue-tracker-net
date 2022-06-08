using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.Commands;
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.Exceptions;
using IssueTracker.Issues.Domain.Services.Version2.Commands.StateChangeCommands;
using MediatR;

namespace IssueTracker.Issues.Domain.Services.Version2.CommandHandlers.StateChangeCommandHandlers;

public sealed class ExecuteCannotReproduceStateChangeCommandHandler : IRequestHandler<ExecuteCannotReproduceStateChangeCommand, Unit>
{
    private readonly IIssueRepository _repository;

    public ExecuteCannotReproduceStateChangeCommandHandler(IIssueRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    /// <inheritdoc />
    public async Task<Unit> Handle(ExecuteCannotReproduceStateChangeCommand request, CancellationToken cancellationToken)
    {
        (IssueIdentifier id, DateTimeOffset stopTime) = request;

        Issue? issue = await _repository.GetByIdOrDefault(id, track: true, cancellationToken);
        if (issue is null)
        {
            throw new IssueNotFoundException(request.Id.ToString());
        }

        if (!issue.Execute(new CannotReproduceStateChangeCommand(stopTime)))
        {
            throw new InvalidStateChangeException(issue.State.Value, typeof(CannotReproduceStateChangeCommand));
        }

        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
