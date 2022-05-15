﻿//
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
using IssueTracker.Issues.Domain.Specifications;

namespace IssueTracker.Issues.Domain.Extensions;

public static class QueryableExtensions
{

    public static IQueryable<TEntity> Where<TEntity>(this IQueryable<TEntity> query,
        PredicateSpecification<TEntity> filterExpression)
        where TEntity : Entity
    {

        return query.Where(filterExpression.Filter);
    }

    public static IQueryable<TEntity> Where<TEntity>(this IQueryable<TEntity> query,
        IEnumerable<PredicateSpecification<TEntity>> filterExpressions)
        where TEntity : Entity
    {
        PredicateSpecification<TEntity>? predicate = filterExpressions
            .Aggregate<PredicateSpecification<TEntity>, PredicateSpecification<TEntity>?>(null, (current, filterExpression) => current is null
                ? filterExpression
                : current.And(filterExpression));

        return predicate is not null
            ? query.Where(predicate.Filter)
            : query;
    }

    public static IQueryable<TResult> Select<TEntity, TResult>(this IQueryable<TEntity> query,
        SelectorSpecification<TEntity, TResult> selectExpression)
        where TEntity : Entity
    {
        return query.Select(selectExpression.Select);
    }
}
