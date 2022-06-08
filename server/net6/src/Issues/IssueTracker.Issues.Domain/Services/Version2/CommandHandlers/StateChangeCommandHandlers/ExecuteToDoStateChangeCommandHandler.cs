using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.Exceptions;
using IssueTracker.Issues.Domain.Services.Version2.Commands.StateChangeCommands;
using MediatR;

namespace IssueTracker.Issues.Domain.Services.Version2.CommandHandlers.StateChangeCommandHandlers;

public sealed class ExecuteToDoStateChangeCommandHandler : IRequestHandler<ExecuteToDoStateChangeCommand, Unit>
{
    private readonly IIssueRepository _repository;

    public ExecuteToDoStateChangeCommandHandler(IIssueRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    /// <inheritdoc />
    public async Task<Unit> Handle(ExecuteToDoStateChangeCommand request, CancellationToken cancellationToken)
    {
        IssueIdentifier id = request.Id;
        Issue? issue = await _repository.GetByIdOrDefault(id, track: true, cancellationToken);
        if (issue is null)
        {
            throw new IssueNotFoundException(request.Id.ToString());
        }


        throw new NotImplementedException();
    }
}
