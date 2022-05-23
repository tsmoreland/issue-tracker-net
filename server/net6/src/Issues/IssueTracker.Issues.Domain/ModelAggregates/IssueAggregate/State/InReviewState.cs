namespace IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.State;

public sealed record InReviewState : IssueState
{
    /// <inheritdoc />
    public override IssueState MoveToNext(bool success) => success
        ? new InTestingState()
        : new OpenState();

    /// <inheritdoc />
    public override IssueState MoveToBacklog() => new BackLogState();

    /// <inheritdoc />
    public override IssueState TryClose(ClosureReason reason) => Close(reason);

    /// <inheritdoc />
    public override IssueState TryOpen() => throw new NotImplementedException();

    /// <inheritdoc />
    public override IssueStateValue Value => IssueStateValue.InReview;
}
