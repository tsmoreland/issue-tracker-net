namespace IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.State;

public sealed record CannotReproduceState : IssueState
{
    /// <inheritdoc />
    public override IssueState MoveToNext(bool success) => this;

    /// <inheritdoc />
    public override IssueState MoveToBacklog() => new BackLogState();

    /// <inheritdoc />
    public override IssueState TryClose(ClosureReason reason) => Close(reason);

    /// <inheritdoc />
    public override IssueState TryOpen() => this;

    /// <inheritdoc />
    public override IssueStateValue Value => IssueStateValue.CannotReproduce;
}
