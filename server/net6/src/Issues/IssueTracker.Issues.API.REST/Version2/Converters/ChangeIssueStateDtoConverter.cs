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

using System.Diagnostics.CodeAnalysis;
using IssueTracker.Issues.API.REST.Version2.DataTransferObjects.Request;
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.Commands;

namespace IssueTracker.Issues.API.REST.Version2.Converters;

internal class ChangeIssueStateDtoConverter
{

    public static bool TryConvertToCommand(IssueIdentifier id, StateChange state, [NotNullWhen(true)] out StateChangeCommand? command)
    {
        command =  state switch
        {
            StateChange.MoveToBackLog => new MoveToBackLogStateChangeCommand(id),
            StateChange.ToDo => new ToDoStateChangeCommand(id),
            StateChange.Open => new OpenStateChangeCommand(id, DateTimeOffset.UtcNow),
            StateChange.Close => new CloseStateChangeCommand(id),
            StateChange.Completed => new CompletedStateChangeCommand(id, DateTimeOffset.UtcNow),
            StateChange.ReadyForReview => new ReadyForReviewStateChangeCommand(id),
            StateChange.ReviewFailed => new ReviewFailedStateChangeCommand(id),
            StateChange.ReadyForTest => new ReadyForTestStateChangeCommand(id),
            StateChange.TestFailed => new TestFailedStateChangeCommand(id),
            StateChange.NotADefect => new NotADefectStateChangeCommand(id, DateTimeOffset.UtcNow),
            StateChange.CannotReproduce => new CannotReproduceStateChangeCommand(id, DateTimeOffset.UtcNow),
            StateChange.WontDo => new WontDoStateChangeCommand(id, DateTimeOffset.UtcNow),
            _ => null,
        };

        return command is not null;
    }
}
