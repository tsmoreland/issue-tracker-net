using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;
using IssueTracker.Issues.Domain.Services.Version2.Commands.StateChangeCommands;
using MediatR;

namespace IssueTracker.Issues.Domain.Services.Version2.CommandHandlers.StateChangeCommandHandlers;

public sealed class ExecuteOpenStateChangeCommandHandler : IRequestHandler<ExecuteOpenStateChangeCommand, Unit>
{
    private readonly IIssueRepository _repository;

    public ExecuteOpenStateChangeCommandHandler(IIssueRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    /// <inheritdoc />
    public Task<Unit> Handle(ExecuteOpenStateChangeCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
