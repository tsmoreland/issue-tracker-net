using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.Commands;

namespace IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.State;

public sealed record NotADefectState : IssueState
{
    /// <inheritdoc />
    public override IssueState Execute(StateChangeCommand command, Issue issue)
    {
        IssueState state = command switch
        {
            CloseStateChangeCommand _ => new ClosedAsNotADefectState(),
            MoveToBackLogStateChangeCommand _ => new BackLogState(),
            _ => this,
        };
        if (!ReferenceEquals(this, state))
        {
            command.UpdateIssue(issue);
        }

        return state;
    }

    /// <inheritdoc />
    public override bool CanExecute(StateChangeCommand command) =>
        command is CloseStateChangeCommand or MoveToBackLogStateChangeCommand;

    /// <inheritdoc />
    public override IssueStateValue Value => IssueStateValue.NotADefect;
}
