//
// Copyright (c) 2023 Terry Moreland
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

/// <summary>
/// base class for Issue State 
/// </summary>
public abstract record class IssueState
{
    /// <inheritdoc cref="Issue.Execute(StateChangeCommand)"/>
    public abstract IssueState Execute(StateChangeCommand command, Issue issue);

    /// <summary>
    /// Checks if <paramref name="command"/> can be run against 
    /// </summary>
    /// <param name="command">the command to check</param>
    /// <returns>
    /// <see langword="true"/> if the command will result in a new state;
    /// otherwise, <see langword="false"/>
    /// </returns>
    public abstract bool CanExecute(StateChangeCommand command);

    /// <summary>
    /// Underlying value
    /// </summary>
    public abstract IssueStateValue Value { get; }

    internal static IssueState Build(IssueStateValue state)
    {
        return state switch
        {
            IssueStateValue.Backlog => new BackLogState(),
            IssueStateValue.ToDo => new ToDoState(),
            IssueStateValue.InProgress => new InProgressState(),
            IssueStateValue.InReview => new InReviewState(),
            IssueStateValue.InTesting => new InTestingState(),
            IssueStateValue.Complete => new CompleteState(),
            IssueStateValue.CannotReproduce => new CannotReproduceState(),
            IssueStateValue.WontDo => new WontDoState(),
            IssueStateValue.NotADefect => new NotADefectState(),
            IssueStateValue.ClosedAsResolved => new ClosedAsResolvedState(),
            IssueStateValue.ClosedAsWontDo => new ClosedAsWontDoState(),
            IssueStateValue.ClosedAsCannotReproduce => new ClosedAsCannotReproduceState(),
            IssueStateValue.ClosedAsNotADefect => new ClosedAsNotADefectState(),
            _ => new BackLogState(),
        };
    }


}
