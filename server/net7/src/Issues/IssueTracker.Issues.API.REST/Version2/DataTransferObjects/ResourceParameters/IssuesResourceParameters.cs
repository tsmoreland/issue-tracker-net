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

namespace IssueTracker.Issues.API.REST.Version2.DataTransferObjects.ResourceParameters;

/// <summary>
/// Grouped Query Parameters for /issues endpoints
/// </summary>
public sealed record class IssuesResourceParameters
{
    /// <summary>
    /// current page number to return
    /// </summary>
    /// <example>1</example>
    public int PageNumber { get; init; }
    /// <summary>
    /// maximum number of items to return
    /// </summary>
    /// <example>10</example>
    public int PageSize { get; init; }
    /// <summary>
    /// order by specification
    /// </summary>
    /// <example>Priority, Type, Title DESC</example>
    public string? OrderBy { get; init; }
    /// <summary>
    /// limit returned issues to those with matching priority values
    /// </summary>
    /// <example>High</example>
    public string?[]? Priorities { get; init; }
    /// <summary>
    /// string value used to limit results by matching against various properties such as title, and description
    /// </summary>
    /// <example>Crash</example>
    public string? SearchQuery { get; init; }

    /// <summary>
    /// Deconstructs the object into its property values
    /// </summary>
    public void Deconstruct(out int pageNumber, out int pageSize, out string? orderBy, out string?[]? priorities, out string? searchQuery)
    {
        pageNumber = PageNumber;
        pageSize = PageSize;
        orderBy = OrderBy;
        priorities = Priorities;
        searchQuery = SearchQuery;
    }

    public object? ToRouteParameters(int? pageNumber = null)
    {
        return new
        {
            pageNumber = pageNumber ?? PageNumber,
            pageSize = PageSize,
            orderBy = OrderBy,
            priorities = Priorities,
            searchQuery =  SearchQuery,
        };
    }
}
