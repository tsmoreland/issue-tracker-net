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

namespace IssueTracker.NetCoreApp21.RestApi.App.Model.Request;

public enum IssueSortBy
{
    /// <summary>
    /// Order by title alphabetically
    /// </summary>
    Title = 0,

    /// <summary>
    /// Order by <see cref="Core.Model.Priority"/>
    /// </summary>
    Priority = 1,

    /// <summary>
    /// Order by <see cref="Core.Model.IssueType"/>
    /// </summary>
    Type = 2,
}

public static class IssueSortByExtensions
{
    public static Core.Model.Issue.SortBy ToSortBy(this IssueSortBy sortBy)
    {
        return (Core.Model.Issue.SortBy)sortBy;
    }
}
