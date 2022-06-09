using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.Commands;

namespace IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.State;

public sealed record InReviewState : IssueState
{
    /// <inheritdoc />
    public override IssueState Execute(StateChangeCommand command, Issue issue)
    {
        IssueState state = command switch
        {
            ReadyForTestStateChangeCommand _ => new InTestingState(),
            ReviewFailedStateChangeCommand _ => new InProgressState(),
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
        command is ReadyForTestStateChangeCommand or ReviewFailedStateChangeCommand;

    /// <inheritdoc />
    public override IssueStateValue Value => IssueStateValue.InReview;
}
