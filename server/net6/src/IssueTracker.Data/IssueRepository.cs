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

using System.Runtime.CompilerServices;
using IssueTracker.Core.Model;
using IssueTracker.Core.Projections;
using IssueTracker.Core.Specifications;
using IssueTracker.Core.Views;
using IssueTracker.Data.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.Data;

public sealed class IssueRepository : IIssueRepository
{
    private readonly IssuesDbContext _dbContext;

    public IssueRepository(IssuesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<IssueSummaryProjection> GetIssueSummaries(int pageNumber, int pageSize, Issue.SortBy sortBy, SortDirection direction, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        IAsyncEnumerable<IssueSummaryProjection> issues = _dbContext.Issues
            .AsNoTracking()
            .Sort(sortBy, direction)
            .Select(i => new IssueSummaryProjection(i.Id, i.Title, i.Priority, i.Type))
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsAsyncEnumerable();
        await foreach (IssueSummaryProjection issue in issues.WithCancellation(cancellationToken))
        {
            yield return issue;
        }
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<IssueSummaryProjection> GetFilteredIssueSummaries(
        IEnumerable<WhereClauseSpecification> filters,
        int pageNumber, int pageSize,
        Issue.SortBy sortBy,
        SortDirection direction,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        IQueryable<Issue> query = _dbContext.Issues
            .AsNoTracking();

        IAsyncEnumerable<IssueSummaryProjection> issues = filters
            .Aggregate(query, (current, specification) => current.Where(specification.Filter))
            .Sort(sortBy, direction)
            .Select(i => new IssueSummaryProjection(i.Id, i.Title, i.Priority, i.Type))
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsAsyncEnumerable();

        await foreach (IssueSummaryProjection issue in issues.WithCancellation(cancellationToken))
        {
            yield return issue;
        }
    }

    /// <inheritdoc />
    public IAsyncEnumerable<Issue> GetIssues(CancellationToken cancellationToken)
    {
        return _dbContext.Issues
            .AsNoTracking()
            .AsAsyncEnumerable();
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<LinkedIssueSummaryProjection> GetParentIssueSummaries(Guid id, int pageNumber, int pageSize, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var summaries = _dbContext.LinkedIssues
            .AsNoTracking()
            .Where(link => link.ChildIssueId == id)
            .Select(li => new {li.ParentIssue.Id, li.ParentIssue.Title, li.ParentIssue.Priority, li.ParentIssue.Type, li.LinkType })
            .OrderBy(i => i.Title)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsAsyncEnumerable();
        await foreach (var summary in summaries.WithCancellation(cancellationToken))
        {
            yield return new LinkedIssueSummaryProjection(summary.Id, summary.Title, summary.Priority, summary.Type, summary.LinkType);
        }
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<LinkedIssueSummaryProjection> GetChildIssueSummaries(Guid id, int pageNumber, int pageSize, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var summaries = _dbContext.LinkedIssues
            .AsNoTracking()
            .Where(link => link.ParentIssueId == id)
            .Select(li => new { li.ChildIssue.Id, li.ChildIssue.Title, li.ChildIssue.Priority, li.ChildIssue.Type, li.LinkType })
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsAsyncEnumerable();
        await foreach (var summary in summaries.WithCancellation(cancellationToken))
        {
            yield return new LinkedIssueSummaryProjection(summary.Id, summary.Title, summary.Priority, summary.Type, summary.LinkType);
        }
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<LinkedIssueView> GetParentIssues(Guid id, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var issues = _dbContext.LinkedIssues
            .AsNoTracking()
            .Where(link => link.ChildIssueId == id)
            .OrderBy(i => i.ChildIssue.Title)
            .Select(li => new { Link = li.LinkType, Issue = li.ParentIssue })
            .AsAsyncEnumerable();
        await foreach (var pair in issues.WithCancellation(cancellationToken))
        {
            yield return new LinkedIssueView(pair.Link, pair.Issue);
        }
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<LinkedIssueView> GetChildIssues(Guid id, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var issues = _dbContext.LinkedIssues
            .AsNoTracking()
            .Where(link => link.ParentIssueId == id)
            .Select(li => new { Link = li.LinkType, Issue = li.ChildIssue })
            .AsAsyncEnumerable();
        await foreach (var pair in issues.WithCancellation(cancellationToken))
        {
            yield return new LinkedIssueView(pair.Link, pair.Issue);
        }
    }

    /// <inheritdoc />
    public Task<Issue?> GetUntrackedIssueById(Guid id, CancellationToken cancellationToken)
    {
        return _dbContext.Issues
            .AsNoTracking()
            .SingleOrDefaultAsync(i => i.Id == id, cancellationToken);
    }

    /// <inheritdoc />
    public Task<Issue?> GetIssueById(Guid id, CancellationToken cancellationToken)
    {
        return _dbContext.Issues
            .FindAsync(new object[] { id }, cancellationToken)
            .AsTask();
    }

    /// <inheritdoc />
    public Task<Issue> AddIssue(Issue issue, CancellationToken cancellationToken)
    {
        _dbContext.Issues.Add(issue);
        return Task.FromResult(issue);
    }

    /// <inheritdoc />
    public async Task<Issue?> UpdateIssue(Guid id, Action<Issue> visitor, CancellationToken cancellationToken)
    {
        Issue? issue = await _dbContext.Issues.FindAsync(new object[] { id }, cancellationToken);
        if (issue == null)
        {
            return null;
        }

        visitor(issue);
        return issue;
    }


    /// <inheritdoc />
    public Task CommitAsync(CancellationToken cancellationToken)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> DeleteIssueById(Guid id, CancellationToken cancellationToken)
    {
        Issue? issue = await _dbContext.Issues.FindAsync(new object[] { id }, cancellationToken);
        if (issue is null)
        {
            return false;
        }

        _dbContext.Issues.Remove(issue);
        return true;

    }

    /// <inheritdoc />
    public Task<bool> IssueExists(Guid id, CancellationToken cancellationToken)
    {
        return _dbContext.Issues
            .AsNoTracking()
            .AnyAsync(i => i.Id == id, cancellationToken);
    }
}
