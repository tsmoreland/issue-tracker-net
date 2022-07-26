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

using MediatR;

namespace IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.Commands;

public abstract record class StateChangeCommand(IssueIdentifier Id)
{
    /// <summary>
    /// Visitor function used to apply additional
    /// settings from commands to issue
    /// </summary>
    /// <param name="issue"><see cref="Issue"/> to visit</param>
    public virtual void UpdateIssue(Issue issue)
{
}
}

public sealed record class MoveToBackLogStateChangeCommand(IssueIdentifier Id) : StateChangeCommand(Id), IRequest<Unit>;
public sealed record class CloseStateChangeCommand(IssueIdentifier Id) : StateChangeCommand(Id), IRequest<Unit>;

public sealed record class ToDoStateChangeCommand(IssueIdentifier Id) : StateChangeCommand(Id), IRequest<Unit>;
public sealed record class ReadyForReviewStateChangeCommand(IssueIdentifier Id) : StateChangeCommand(Id), IRequest<Unit>;
public sealed record class ReviewFailedStateChangeCommand(IssueIdentifier Id) : StateChangeCommand(Id), IRequest<Unit>;
public sealed record class ReadyForTestStateChangeCommand(IssueIdentifier Id) : StateChangeCommand(Id), IRequest<Unit>;
public sealed record class TestFailedStateChangeCommand(IssueIdentifier Id) : StateChangeCommand(Id), IRequest<Unit>;

