using IssueTracker.Issues.API.Version2.Abstractions.DataTransferObjects;
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;
using MediatR;

namespace IssueTracker.Issues.API.Version2.Abstractions.Commands;

public sealed record class CreateIssueCommand(
    string Project,
    string Title,
    string Description,
    Priority Priority,
    IssueType Type,
    IssueIdentifier? EpicId) : IRequest<IssueDto>;
