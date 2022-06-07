﻿using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.Commands;

namespace IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.State;

public sealed record ClosedAsWontDoState : IssueState
{
    /// <inheritdoc />
    public override IssueState Execute(StateChangeCommand command, Issue issue)
    {
        return command switch
        {
            MoveToBackLogStateChangeCommand _ => new BackLogState(),
            _ => this,
        };
    }

    /// <inheritdoc />
    public override bool CanExecute(StateChangeCommand command) =>
        command is MoveToBackLogStateChangeCommand;

    /// <inheritdoc />
    public override IssueStateValue Value => IssueStateValue.ClosedAsWontDo;
}
