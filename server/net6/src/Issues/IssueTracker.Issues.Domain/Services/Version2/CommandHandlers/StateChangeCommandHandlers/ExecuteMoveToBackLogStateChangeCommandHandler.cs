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

using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.Commands;
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.Exceptions;
using IssueTracker.Issues.Domain.Services.Version2.Commands.StateChangeCommands;
using MediatR;

namespace IssueTracker.Issues.Domain.Services.Version2.CommandHandlers.StateChangeCommandHandlers;

public sealed class ExecuteMoveToBackLogStateChangeCommandHandler : IRequestHandler<ExecuteMoveToBackLogStateChangeCommand, Unit>
{
    private readonly IIssueRepository _repository;

    public ExecuteMoveToBackLogStateChangeCommandHandler(IIssueRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    /// <inheritdoc />
    public async Task<Unit> Handle(ExecuteMoveToBackLogStateChangeCommand request, CancellationToken cancellationToken)
    {
        Issue? issue = await _repository.GetByIdOrDefault(request.Id, track: true, cancellationToken);
        if (issue is null)
        {
            throw new IssueNotFoundException(request.Id.ToString());
        }

        if (!issue.Execute(new MoveToBackLogStateChangeCommand()))
        {
            throw new InvalidStateChangeException(issue.State.Value, typeof(MoveToBackLogStateChangeCommand));
        }

        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
