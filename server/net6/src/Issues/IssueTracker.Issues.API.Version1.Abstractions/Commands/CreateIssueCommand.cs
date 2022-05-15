using IssueTracker.Issues.API.Version1.Abstractions.DataTransferObjects;
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;
using MediatR;

namespace IssueTracker.Issues.API.Version1.Abstractions.Commands;

public sealed record class CreateIssueCommand(
    string Project,
    string Title,
    string Description,
    Priority Priority) : IRequest<IssueDto>;
