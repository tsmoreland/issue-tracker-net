//
// Copyright © 2022 Terry Moreland
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.Commands;

namespace IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.State;

public sealed record class BackLogState : IssueState
{
    /// <inheritdoc />
    public override IssueState Execute(StateChangeCommand command, Issue issue)
    {
        IssueState state = command switch
        {
            WontDoStateChangeCommand _ => new WontDoState(),
            CannotReproduceStateChangeCommand _ => new CannotReproduceState(),
            NotADefectStateChangeCommand _ => new NotADefectState(),
            OpenStateChangeCommand _ => new InProgressState(),
            ToDoStateChangeCommand _ => new ToDoState(),
            _ => this,
        };

        if (!ReferenceEquals(state, this))
        {
            command.UpdateIssue(issue);
        }

        return state;

    }

    /// <inheritdoc />
    public override bool CanExecute(StateChangeCommand command) =>
        command is
            NotADefectStateChangeCommand or
            WontDoStateChangeCommand or
            CannotReproduceStateChangeCommand or
            OpenStateChangeCommand or
            ToDoStateChangeCommand;

    /// <inheritdoc />
    public override IssueStateValue Value => IssueStateValue.Backlog;
}
