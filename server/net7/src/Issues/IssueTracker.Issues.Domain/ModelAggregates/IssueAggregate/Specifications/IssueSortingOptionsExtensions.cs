//
// Copyright (c) 2022 Terry Moreland
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
using IssueTracker.Issues.Domain.ModelAggregates.Specifications;
using IssueTracker.Issues.Domain.Services.Version2.DataTransferObjects;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.Specifications;

public static class IssueSortingOptionsExtensions
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
        return property.ToUpperInvariant() switch
        {
            "ID" =>
                issue => issue.Id,
            "TITLE" =>
                issue => EF.Property<string>(issue, "_title"),
            "DESCRIPTION" =>
                issue => EF.Property<string>(issue, "_description"),
            "EPICID" =>
                issue => EF.Property<IssueIdentifier>(issue, "_epicId"),
            "PROJECT" =>
                issue => EF.Property<string>(issue, "_projectId"),
            "ISSUENUMBER" =>
                issue => EF.Property<int>(issue, "_issueNumber"),
            "TYPE" =>
                issue => EF.Property<IssueType>(issue, "_type"),
            "PRIORITY" =>
                issue => issue.Priority,
            "ASSIGNEE" =>
                issue => issue.Assignee != null ? issue.Assignee.FullName : User.Unassigned.FullName,
            "REPORTER" =>
                issue => EF.Property<string>(issue, "_reporter.FullName"),
            _ =>
                issue => issue.Id,
        };
    }
}
