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

using IssueTracker.Issues.Domain.ModelAggregates.Specifications;

namespace IssueTracker.Issues.Domain.DataContracts;

public interface IRepository<in TIdentity, TEntity> where TEntity : Entity
{
    IUnitOfWork UnitOfWork { get; }

    [Obsolete("Doesn't work with Sqlite provider due to lack of support for Default if empty")]
    Task<T?> Max<T>(
        IPredicateSpecification<TEntity> filterExpression,
        ISelectorSpecification<TEntity, T> selectExpression,
        CancellationToken cancellationToken = default);

    ValueTask<TEntity?> GetByIdOrDefault(TIdentity id, bool track = true, CancellationToken cancellationToken = default);

    Task<TEntity?> GetByFilter(IPredicateSpecification<TEntity> filterExpression,
        bool track = true,
        CancellationToken cancellationToken = default);

    public Task<T?> GetProjection<T>(
        ISelectorSpecification<TEntity, T> selectExpression,
        CancellationToken cancellationToken = default) =>
        GetProjectionByFilter<T>(IPredicateSpecification<TEntity>.None, selectExpression, cancellationToken);


    Task<T?> GetProjectionByFilter<T>(
        IPredicateSpecification<TEntity> filterExpression,
        ISelectorSpecification<TEntity, T> selectExpression,
        CancellationToken cancellationToken = default);

    public Task<(int Total, IAsyncEnumerable<T> Collection)> GetPagedAndSortedProjections<T>(
        ISelectorSpecification<TEntity, T> selectExpression,
        PagingOptions paging, SortingOptions sorting,
        CancellationToken cancellationToken = default) =>
        GetPagedAndSortedProjections(
            IPredicateSpecification<TEntity>.None,
            selectExpression,
            paging, sorting,
            cancellationToken);

    Task<(int Total, IAsyncEnumerable<T> Collection)> GetPagedAndSortedProjections<T>(
        IPredicateSpecification<TEntity> filterExpression,
        ISelectorSpecification<TEntity, T> selectExpression,
        PagingOptions paging, SortingOptions sorting,
        CancellationToken cancellationToken = default);

    Task<bool> Exists(TIdentity id, CancellationToken cancellationToken = default);
    Task<bool> DeleteById(TIdentity id, CancellationToken cancellationToken = default);
}
