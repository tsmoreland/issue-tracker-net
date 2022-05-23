namespace IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.State;

public sealed record ClosedAsWontDoState : IssueState
{
    /// <inheritdoc />
    public override IssueState MoveToNext(bool success) => this;

    /// <inheritdoc />
    public override IssueState MoveToBacklog() => this;

    /// <inheritdoc />
    public override IssueState TryClose(ClosureReason reason) => this;

    /// <inheritdoc />
    public override IssueState TryOpen() => new OpenState();

    /// <inheritdoc />
    public override IssueStateValue Value => IssueStateValue.ClosedAsWontDo;
}
