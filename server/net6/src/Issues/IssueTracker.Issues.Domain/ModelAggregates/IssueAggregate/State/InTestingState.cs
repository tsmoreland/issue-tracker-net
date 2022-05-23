namespace IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.State;

public sealed record InTestingState : IssueState
{
    /// <inheritdoc />
    public override IssueState MoveToNext(bool success) => success
        ? new CompleteState()
        : new OpenState();

    /// <inheritdoc />
    public override IssueState MoveToBacklog() => new BackLogState();

    /// <inheritdoc />
    public override IssueState TryClose(ClosureReason reason) => Close(reason);

    /// <inheritdoc />
    public override IssueState TryOpen() => this;

    /// <inheritdoc />
    public override IssueStateValue Value => IssueStateValue.InTesting;
}
