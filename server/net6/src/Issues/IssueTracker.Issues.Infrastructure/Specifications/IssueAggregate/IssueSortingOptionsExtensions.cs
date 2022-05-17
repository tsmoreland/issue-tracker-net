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

using System.Linq.Expressions;
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;
using IssueTracker.Issues.Domain.Specifications;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.Issues.Infrastructure.Specifications.IssueAggregate;

internal static class IssueSortingOptionsExtensions
{
    public static IOrderedQueryable<Issue> OrderUsing(this IQueryable<Issue> query, SortingOptions sorting)
    {
        (string orderByProperty, IEnumerable<string>? thenByProperties, bool ascending) = sorting;
        IOrderedQueryable<Issue> orderedQuery = ascending
            ? query.OrderBy(i => i.Id) // order by id by default 
            : query.OrderByDescending(i => i.Id);


        if (sorting == SortingOptions.Empty)
        {
            return query.OrderBy(i => i.Id);
        }

        Expression<Func<Issue, object>>? orderBy = GenerateOrderExpression(orderByProperty);
        orderedQuery = ascending
            ? orderedQuery.OrderBy(orderBy)
            : orderedQuery.OrderByDescending(orderBy);

        if (thenByProperties is null)
        {
            return orderedQuery;
        }

        if (ascending)
        {
            orderedQuery = thenByProperties
                .Select(GenerateOrderExpression)
                .Aggregate(
                    orderedQuery,
                    (current, thenBy) => current.ThenBy(thenBy));
        }
        else
        {
            orderedQuery = thenByProperties
                .Select(GenerateOrderExpression)
                .Aggregate(
                    orderedQuery,
                    (current, thenBy) => current.ThenByDescending(thenBy));
        }

        return orderedQuery;
    }

    private static Expression<Func<Issue, object>> GenerateOrderExpression(string property)
    {
        return property switch
        {
            nameof(Issue.Id) =>
                issue => issue.Id,
            nameof(Issue.Title) =>
                issue => EF.Property<string>(issue, "_title"),
            nameof(Issue.Description) =>
                issue => EF.Property<string>(issue,"_description"),
            nameof(Issue.EpicId) =>
                issue => EF.Property<IssueIdentifier>(issue, "_epicId"),
            nameof(Issue.Project) =>
                issue => EF.Property<string>(issue, "_project"),
            nameof(Issue.IssueNumber) =>
                issue => EF.Property<int>(issue, "_issueNumber"),
            nameof(Issue.Type) =>
                issue => EF.Property<IssueType>(issue, "_type"),
            nameof(Issue.Assignee) =>
                issue => EF.Property<string>(issue, "_assignee.FullName"),
            nameof(Issue.Reporter) =>
                issue => EF.Property<string>(issue, "_reporter.FullName"),
            _ =>
                issue => issue.Id,
        };
    }
}
