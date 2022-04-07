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
    public async IAsyncEnumerable<IssueSummaryProjection> GetIssueSummaries(int pageNumber, int pageSize, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        IAsyncEnumerable<IssueSummaryProjection> issues = _dbContext.Issues
            .AsNoTracking()
            .Select(i => new IssueSummaryProjection(i.Id, i.Title))
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsAsyncEnumerable();
        await foreach (IssueSummaryProjection issue in issues.WithCancellation(cancellationToken))
        {
            yield return issue;
        }
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<IssueSummaryProjection> GetParentIssues(Guid id, int pageNumber, int pageSize, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        IAsyncEnumerable<IssueSummaryProjection> issues = _dbContext.LinkedIssues
            .AsNoTracking()
            .Where(link => link.ParentIssueId == id)
            .Select(link => link.ChildIssue)
            .Select(i => new IssueSummaryProjection(i.Id, i.Title))
            .OrderBy(i => i.Title)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsAsyncEnumerable();
        await foreach (IssueSummaryProjection issue in issues.WithCancellation(cancellationToken))
        {
            yield return issue;
        }

    }

    /// <inheritdoc />
    public async IAsyncEnumerable<IssueSummaryProjection> GetChildIssues(Guid id, int pageNumber, int pageSize, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        IAsyncEnumerable<IssueSummaryProjection> issues = _dbContext.LinkedIssues
            .AsNoTracking()
            .Where(link => link.ChildIssueId == id)
            .Select(link => link.ParentIssue)
            .Select(i => new IssueSummaryProjection(i.Id, i.Title))
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsAsyncEnumerable();
        await foreach (IssueSummaryProjection issue in issues.WithCancellation(cancellationToken))
        {
            yield return issue;
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
            .SingleOrDefaultAsync(i => i.Id == id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Issue> AddIssue(Issue issue, CancellationToken cancellationToken)
    {
        _dbContext.Issues.Add(issue);
        await _dbContext.SaveChangesAsync(cancellationToken);
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
