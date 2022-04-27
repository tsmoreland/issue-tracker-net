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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IssueTracker.Core.Model;
using IssueTracker.Core.Projections;
using IssueTracker.Data.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.EFCore21.Data;

public sealed class IssueRepository : IIssueRepository
{
    private readonly IssuesDbContext _dbContext;

    public IssueRepository(IssuesDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<IssueSummaryProjection>> GetIssueSummaries(int pageNumber, int pageSize, Issue.SortBy sortBy, SortDirection direction,
        CancellationToken cancellationToken)
    {
        return await _dbContext.Issues
            .AsNoTracking()
            .Select(i => new IssueSummaryProjection(i.Id, i.Title, i.Priority, i.Type))
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<LinkedIssueSummaryProjection>> GetParentIssueSummaries(int id, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        return await _dbContext.LinkedIssues
            .AsNoTracking()
            .Where(link => link.ChildIssueId == id)
            .Select(li => new
            {
                li.ParentIssue.Id,
                li.ParentIssue.Title,
                li.ParentIssue.Priority,
                li.ParentIssue.Type,
                li.LinkType
            })
            .OrderBy(i => i.Title)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToAsyncEnumerable()
            .Select(summary =>
                new LinkedIssueSummaryProjection(summary.Id, summary.Title, summary.Priority, summary.Type,
                    summary.LinkType))
            .ToList(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<LinkedIssueSummaryProjection>> GetChildIssueSummaries(int id, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        return await _dbContext.LinkedIssues
            .AsNoTracking()
            .Where(link => link.ParentIssueId == id)
            .Select(li => new
            {
                li.ChildIssue.Id,
                li.ChildIssue.Title,
                li.ChildIssue.Priority,
                li.ChildIssue.Type,
                li.LinkType
            })
            .OrderBy(i => i.Title)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToAsyncEnumerable()
            .Select(summary =>
                new LinkedIssueSummaryProjection(summary.Id, summary.Title, summary.Priority, summary.Type,
                    summary.LinkType))
            .ToList(cancellationToken);
    }

    /// <inheritdoc />
    public Task<Issue> GetUntrackedIssueById(int id, CancellationToken cancellationToken)
    {
        return _dbContext.Issues
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
    }

    /// <inheritdoc />
    public Task<Issue> GetIssueById(int id, CancellationToken cancellationToken)
    {
        return _dbContext.Issues
            .FindAsync(new object[] { id }, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Issue> AddIssue(Issue issue, CancellationToken cancellationToken)
    {
        return (await _dbContext.Issues.AddAsync(issue, cancellationToken)).Entity;
    }

    /// <inheritdoc />
    public Task<Issue> UpdateIssue(int id, Action<Issue> visitor, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public async Task<bool> DeleteIssueById(int id, CancellationToken cancellationToken)
    {
        Issue issue = await _dbContext.Issues.FindAsync(id, cancellationToken);
        if (issue is null)
        {
            return false;
        }
        _dbContext.Issues.Remove(issue);
        return true;
    }

    /// <inheritdoc />
    public Task<bool> IssueExists(int id, CancellationToken cancellationToken)
    {
        return _dbContext.Issues.AnyAsync(i => i.Id == id, cancellationToken);
    }

    /// <inheritdoc />
    public Task CommitAsync(CancellationToken cancellationToken)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
