namespace IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.State;

public sealed record NotADefectState : IssueState
{
    /// <param name="success"></param>
    /// <inheritdoc />
    public override IssueState MoveToNext(bool success) => this;

    /// <inheritdoc />
    public override IssueState MoveToBacklog() => new BackLogState();

    /// <inheritdoc />
    public override IssueState TryClose(ClosureReason reason) => throw new NotImplementedException();

    /// <inheritdoc />
    public override IssueState TryOpen() => this;

    /// <inheritdoc />
    public override IssueStateValue Value => IssueStateValue.NotADefect;
}
