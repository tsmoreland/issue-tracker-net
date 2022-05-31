using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.Commands;

namespace IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.State;

public sealed record InTestingState : IssueState
{
    /// <inheritdoc />
    public override IssueState Execute(StateChangeCommand command)
    {
        return command switch
        {
            CompletedStateChangeCommand _ => new CompleteState(),
            TestFailedStateChangeCommand _ => new InProgressState(),
            _ => this,
        };
    }

    /// <inheritdoc />
    public override bool CanExecute(StateChangeCommand command) =>
        command is CompletedStateChangeCommand or TestFailedStateChangeCommand;

    /// <inheritdoc />
    public override IssueStateValue Value => IssueStateValue.InTesting;
}
