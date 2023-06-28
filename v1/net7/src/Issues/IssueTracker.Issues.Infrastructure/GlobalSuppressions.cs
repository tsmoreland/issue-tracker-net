// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Minor Code Smell", "S1905:Redundant casts should not be used", Justification = "Cast is required to add nullable attribute", Scope = "member", Target = "~M:IssueTracker.Issues.Infrastructure.Repositories.IssueRepository.GetByIdOrDefault(IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.IssueIdentifier,System.Boolean,System.Threading.CancellationToken)~System.Threading.Tasks.ValueTask{IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.Issue}")]
