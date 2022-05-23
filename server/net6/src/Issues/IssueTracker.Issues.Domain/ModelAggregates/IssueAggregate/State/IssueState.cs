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

namespace IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.State;

/// <summary>
/// base class for Issue State 
/// </summary>
public abstract record class IssueState
{
    /// <inheritdoc cref="Issue.MoveStateToNext(bool)"/>
    public abstract IssueState MoveToNext(bool success);

    /// <inheritdoc cref="Issue.MoveToBacklog()"/>
    public abstract IssueState MoveToBacklog();

    /// <inheritdoc cref="Issue.TryClose(ClosureReason)"/>
    public abstract IssueState TryClose(ClosureReason reason);

    /// <inheritdoc cref="Issue.TryOpen()"/>
    public abstract IssueState TryOpen();

    /// <summary>
    /// Underlying value
    /// </summary>
    public abstract IssueStateValue Value { get; }

    /// <summary>
    /// moves to closed state using <paramref name="reason"/>
    /// </summary>
    protected IssueState Close(ClosureReason reason)
    {
        return reason switch
        {
            ClosureReason.CannotReproduce => new ClosedAsCannotReproduceState(),
            ClosureReason.Deferred => new ClosedAsDeferredState(),
            ClosureReason.Resolved => new ClosedAsResolvedState(),
            ClosureReason.NotADefect => new ClosedAsNotADefectState(),
            ClosureReason.WontDo => new ClosedAsWontDoState(),
            _ => this,
        };
    }

    internal static IssueState Build(IssueStateValue state)
    {
        return state switch
        {
            IssueStateValue.Backlog => new BackLogState(),
            IssueStateValue.ToDo => new ToDoState(),
            IssueStateValue.Open => new OpenState(),
            IssueStateValue.InReview => new InReviewState(),
            IssueStateValue.WontDo => new WontDoState(),
            IssueStateValue.CannotReproduce => new CannotReproduceState(),
            IssueStateValue.NotADefect => new NotADefectState(),
            IssueStateValue.InTesting => new InTestingState(),
            IssueStateValue.Complete => new CompleteState(),
            IssueStateValue.ClosedAsResolved => new ClosedAsResolvedState(),
            IssueStateValue.ClosedAsDeferred => new ClosedAsDeferredState(),
            IssueStateValue.ClosedAsWontDo => new ClosedAsWontDoState(),
            IssueStateValue.ClosedAsCannotReproduce => new ClosedAsCannotReproduceState(),
            IssueStateValue.ClosedAsNotADefect => new ClosedAsNotADefectState(),
            _ => new BackLogState(),
        };
    }
}
