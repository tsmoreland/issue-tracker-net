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

using System.Linq.Expressions;
using IssueTracker.Core.Model;
using IssueTracker.Core.ValueObjects;

namespace IssueTracker.Data;

internal static class QueryableExtensions
{
    public static IQueryable<Issue> Sort(
        this IQueryable<Issue> query,
        Issue.SortBy sortBy,
        SortDirection direction)
    {
        return direction switch
        {
            SortDirection.Descending => query.OrderBy(GetFilter(sortBy)),
            _ => query.OrderByDescending(GetFilter(sortBy)),
        };
    }

    private static Expression<Func<Issue, object?>> GetFilter(Issue.SortBy sortBy)
    {
        return sortBy switch
        {
            Issue.SortBy.Title => issue => (object?) issue.Title,
            Issue.SortBy.Priority => issue => (object?) issue.Priority,
            Issue.SortBy.Type => issue => (object?) issue.Type,
            _ => issue => (object?) issue.Title,
        };
    }
}
