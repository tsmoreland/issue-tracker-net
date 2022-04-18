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
using IssueTracker.Core.Requests;
using IssueTracker.GrpcApi.Grpc.Services;

namespace IssueTracker.GrpcApi.Services;

internal static class RequestMessageExtensions
{
    public static GetPagedAndSortedIssuesRequest ToMediatorRequest(this PagedIssueRequestMessage request)
    {
        return new GetPagedAndSortedIssuesRequest(
            request.PageNumber,
            request.PageSize,
            request.SortBy.ToIssueSortBy(),
            request.Direction.ToSortDirection());
    }

    public static GetPagedAndSortedIssuesRequest ToMediatorRequest(this SortedIssueRequestMessage request)
    {
        return new GetPagedAndSortedIssuesRequest(
            1,
            int.MaxValue,
            request.SortBy.ToIssueSortBy(),
            request.Direction.ToSortDirection());

    }

    public static Issue.SortBy ToIssueSortBy(this IssueSortBy value)
    {
        return value switch
        {
            IssueSortBy.Title => Issue.SortBy.Title,
            IssueSortBy.Priority => Issue.SortBy.Priority,
            //IssueSortBy.Type => Issue.SortBy.Type,  // not yet added to proto
            _ => Issue.SortBy.Title,
        };
    }

    public static SortDirection ToSortDirection(this SortOrder value)
    {
        return value switch
        {
            SortOrder.Descending => SortDirection.Descending,
            SortOrder.Ascending => SortDirection.Ascending,
            _ => SortDirection.Ascending,
        };
    }
}
