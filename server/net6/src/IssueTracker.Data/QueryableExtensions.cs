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

using IssueTracker.Core.Model;

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
            SortDirection.Descending => sortBy switch
            {
                Issue.SortBy.Title => query.OrderByDescending(i => i.Title),
                Issue.SortBy.Priority => query.OrderByDescending(i => i.Priority),
                Issue.SortBy.Type => query.OrderByDescending(i => i.Type),
                _ => query.OrderByDescending(i => i.Title)
            },
            /* Ascending */ _ => sortBy switch
            {
                Issue.SortBy.Priority => query.OrderBy(i => i.Priority),
                Issue.SortBy.Type => query.OrderBy(i => i.Type),
                /* Title */_ => query.OrderBy(i => i.Title)
            }
        };
    }
}
