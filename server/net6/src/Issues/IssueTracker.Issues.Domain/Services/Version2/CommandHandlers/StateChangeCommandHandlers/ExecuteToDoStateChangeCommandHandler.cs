﻿using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;
using IssueTracker.Issues.Domain.Services.Version2.Commands.StateChangeCommands;
using MediatR;

namespace IssueTracker.Issues.Domain.Services.Version2.CommandHandlers.StateChangeCommandHandlers;

public sealed class ExecuteToDoStateChangeCommandHandler : IRequestHandler<ExecuteToDoStateChangeCommand, Unit>
{
    private readonly IIssueRepository _repository;

    public ExecuteToDoStateChangeCommandHandler(IIssueRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    /// <inheritdoc />
    public Task<Unit> Handle(ExecuteToDoStateChangeCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
