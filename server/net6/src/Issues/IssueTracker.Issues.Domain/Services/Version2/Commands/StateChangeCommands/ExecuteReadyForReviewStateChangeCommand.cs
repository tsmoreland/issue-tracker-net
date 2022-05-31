﻿using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;
using MediatR;

namespace IssueTracker.Issues.Domain.Services.Version2.Commands.StateChangeCommands;

public sealed record class ExecuteReadyForReviewStateChangeCommand(IssueIdentifier Id) : IRequest<Unit>;
