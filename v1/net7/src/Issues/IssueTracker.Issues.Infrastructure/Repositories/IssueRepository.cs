﻿//
// Copyright (c) 2023 Terry Moreland
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
using IssueTracker.Issues.Domain.Extensions;
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.Specifications;
using IssueTracker.Issues.Domain.ModelAggregates.Specifications;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.Issues.Infrastructure.Repositories;

public sealed class IssueRepository : IIssueRepository
{
    private readonly IssuesDbContext _dbContext;

    public IssueRepository(IssuesDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public IUnitOfWork UnitOfWork => _dbContext;

    /// <inheritdoc />
    public ValueTask<Project?> FindProjectById(string id, CancellationToken cancellationToken)
    {
        return _dbContext.Projects.FindAsync(new object?[] { id }, cancellationToken);
    }

    /// <inheritdoc />
    public Issue Add(Issue issue)
    {
        return _dbContext.Issues.Add(issue).Entity;
    }

    /// <inheritdoc />
    public Issue Update(Issue issue)
    {
        _dbContext.Entry(issue).State = EntityState.Modified;
        return _dbContext.Entry(issue).Entity;
    }

    /// <inheritdoc />
    public ValueTask<int> MaxIssueNumber(string project, CancellationToken cancellationToken = default)
    {
        SelectIssueNumber selector = new();
        return _dbContext.Issues.AsNoTracking()
            .Where(new ProjectMatchesPredicate(project).Filter)
            .OrderByDescending(selector.Select)
            .Select(selector.Select)
            .Take(1)
            .AsAsyncEnumerable()
            .DefaultIfEmpty(0)
            .FirstOrDefaultAsync(cancellationToken);
    }

    [Obsolete("default if empty doesn't work for sqlite based on experimentation")]
    public Task<T?> Max<T>(IPredicateSpecification<Issue> filterExpression, ISelectorSpecification<Issue, T> selectExpression, CancellationToken cancellationToken = default)
    {
        return _dbContext.Issues.AsNoTracking()
            .AsNoTracking()
            .Include("_children")
            .Include("_parents")
            .Where(filterExpression)
            .Select(selectExpression)
            .DefaultIfEmpty(default)
            .MaxAsync(cancellationToken);
    }

    public async ValueTask<Issue?> GetByIdOrDefault(IssueIdentifier id, bool track = true, CancellationToken cancellationToken = default)
    {
        if (!track)
        {
            return await _dbContext.Issues
                .AsNoTracking()
                .Include("_children")
                .Include("_parents")
                .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
        }

        Issue? issue = await _dbContext.Issues.FindAsync(new object[] { id }, cancellationToken);
        if (issue is null)
        {
            return issue;
        }

        Task relatedToTask = _dbContext.Entry(issue).Collection("_children").LoadAsync(cancellationToken);
        Task relatedFromTask = _dbContext.Entry(issue).Collection("_parents").LoadAsync(cancellationToken);

        await Task.WhenAll(relatedToTask, relatedFromTask);

        return issue;
    }

    public Task<Issue?> GetByFilter(IPredicateSpecification<Issue> filterExpression,
        bool track = true,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Issue> query = _dbContext.Issues;
        if (!track)
        {
            query = query.AsNoTracking();
        }

        return query.Where(filterExpression).FirstOrDefaultAsync(cancellationToken);
    }

    public Task<T?> GetProjectionByFilter<T>(
        IPredicateSpecification<Issue> filterExpression,
        ISelectorSpecification<Issue, T> selectExpression,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Issues.AsNoTracking()
            .Where(filterExpression)
            .Select(selectExpression)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<(int TotalPages, int TotalCount, IAsyncEnumerable<T> Collection)> GetPagedAndSortedProjections<T>(
        IPredicateSpecification<Issue> filterExpression,
        ISelectorSpecification<Issue, T> selectExpression,
        PagingOptions paging, SortingOptions sorting,
        CancellationToken cancellationToken = default)
    {

        int totalCount = await _dbContext.Issues.AsNoTracking()
            .Where(filterExpression)
            .CountAsync(cancellationToken);
        int totalPages = (int)Math.Ceiling(totalCount / (double)paging.PageSize);

        return (totalPages, totalCount, _dbContext.Issues.AsNoTracking()
            .Include("_children")
            .Include("_parents")
            .Where(filterExpression)
            .OrderUsing(sorting)
            .Select(selectExpression)
            .Skip(paging.Skip)
            .Take(paging.Take)
            .AsAsyncEnumerable());
    }

    public Task<bool> Exists(IssueIdentifier id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Issues.AsNoTracking()
            .AnyAsync(i => i.Id == id, cancellationToken);
    }

    public Task<bool> DeleteById(IssueIdentifier id, CancellationToken cancellationToken = default)
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
