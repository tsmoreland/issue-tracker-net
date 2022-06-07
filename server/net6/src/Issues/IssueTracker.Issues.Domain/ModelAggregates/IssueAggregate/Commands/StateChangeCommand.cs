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

namespace IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.Commands;

public abstract record class StateChangeCommand;

public sealed record class MoveToBackLogStateChangeCommand : StateChangeCommand;
public sealed record class CloseStateChangeCommand : StateChangeCommand;
public sealed record class WontDoStateChangeCommand(DateTimeOffset StopTime) : StateChangeCommand;
public sealed record class NotADefectStateChangeCommand(DateTimeOffset StopTime) : StateChangeCommand;
public sealed record class CannotReproduceStateChangeCommand(DateTimeOffset StopTime) : StateChangeCommand;
public sealed record class OpenStateChangeCommand(DateTimeOffset StartTime) : StateChangeCommand;
public sealed record class ToDoStateChangeCommand : StateChangeCommand;
public sealed record class ReadyForReviewStateChangeCommand : StateChangeCommand;
public sealed record class ReviewFailedStateChangeCommand : StateChangeCommand;
public sealed record class ReadyForTestStateChangeCommand : StateChangeCommand;
public sealed record class TestFailedStateChangeCommand : StateChangeCommand;
public sealed record class CompletedStateChangeCommand(DateTimeOffset StopTime) : StateChangeCommand;

