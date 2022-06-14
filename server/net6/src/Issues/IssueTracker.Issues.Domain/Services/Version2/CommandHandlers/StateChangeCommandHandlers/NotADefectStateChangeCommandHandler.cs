using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.Commands;
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.Exceptions;
using MediatR;

namespace IssueTracker.Issues.Domain.Services.Version2.CommandHandlers.StateChangeCommandHandlers;

public sealed class NotADefectStateChangeCommandHandler : IRequestHandler<NotADefectStateChangeCommand, Unit>
{
    private readonly IIssueRepository _repository;

    public NotADefectStateChangeCommandHandler(IIssueRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    /// <inheritdoc />
    public async Task<Unit> Handle(NotADefectStateChangeCommand request, CancellationToken cancellationToken)
    {
        Issue? issue = await _repository.GetByIdOrDefault(request.Id, track: true, cancellationToken);
        if (issue is null)
        {
            throw new IssueNotFoundException(request.Id.ToString());
        }

        if (!issue.Execute(request))
        {
            throw new InvalidStateChangeException(issue.State.Value, typeof(NotADefectStateChangeCommand));
        }

        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
