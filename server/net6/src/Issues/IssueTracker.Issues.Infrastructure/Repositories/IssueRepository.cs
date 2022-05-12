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

using IssueTracker.Issues.Domain.DataContracts;
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;
using IssueTracker.Issues.Domain.Specifications;
using IssueTracker.Issues.Domain.Extensions;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.Issues.Infrastructure.Repositories;

public sealed class IssueRepository 
{
    private readonly IssuesDbContext _dbContext;

    public IssueRepository(IssuesDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public IUnitOfWork UnitOfWork => _dbContext;

    public Task<T> Max<T>(IEnumerable<WhereClauseSpecification<Issue>> filterExpressions, SelectorSpecification<Issue, T> selectExpression, CancellationToken cancellationToken = default)
    {
        return _dbContext.Issues.AsNoTracking()
            .Where(filterExpressions)
            .MaxAsync(selectExpression.Select, cancellationToken);
    }

    public ValueTask<Issue?> GetIssueByIdOrDefault(IssueIdentifier id, bool track = true, CancellationToken cancellationToken = default)
    {
        return track
            ? _dbContext.Issues.FindAsync(new object[] { id }, cancellationToken)
            : new ValueTask<Issue?>(_dbContext.Issues.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id, cancellationToken));
    }

    public Task<Issue?> GetIssueByFilter(IEnumerable<WhereClauseSpecification<Issue>> filterExpressions,
        bool track = true,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Issue> query = _dbContext.Issues;
        if (!track)
        {
            query = query.AsNoTracking();
        }

        return query.Where(filterExpressions).FirstOrDefaultAsync(cancellationToken);
    }

    public IAsyncEnumerable<T> GetPagedAndSortedCollection<T>(IEnumerable<WhereClauseSpecification<Issue>> filterExpressions,
        SelectorSpecification<Issue, T> selectExpression, PageSpecification paging)
    {
        return _dbContext.Issues.AsNoTracking()
            .Where(filterExpressions)
            .Select(selectExpression)
            .Skip(paging.Skip)
            .Take(paging.Take)
            .AsAsyncEnumerable();
    }

    public Task<bool> IssueExists(IssueIdentifier id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Issues.AsNoTracking()
            .AnyAsync(i => i.Id == id, cancellationToken);
    }

    public Task<bool> DeleteIssueById(IssueIdentifier id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Issues
            .FindAsync(new object[] { id }, cancellationToken)
            .AsTask()
            .ContinueWith(findTask =>
            {
                Issue? issue = findTask.Result;
                if (issue is not null)
                {
                    _dbContext.Issues.Remove(issue);
                    return true;
                }
                else
                {
                    return false;
                } 
            }, cancellationToken);
    }
}
