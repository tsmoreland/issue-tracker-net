using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.Commands;

namespace IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.State;

public sealed record ClosedAsNotADefectState : IssueState
{
    /// <inheritdoc />
    public override IssueState Execute(StateChangeCommand command, Issue issue)
    {
        return command switch
        {
            OpenStateChangeCommand _ => new InProgressState(),
            _ => this,
        };
    }

    /// <inheritdoc />
    public override bool CanExecute(StateChangeCommand command) =>
        command is OpenStateChangeCommand;

    /// <inheritdoc />
    public override IssueStateValue Value => IssueStateValue.ClosedAsNotADefect;
}
