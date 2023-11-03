//
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

using System.ComponentModel.DataAnnotations;
using AutoMapper;
using IssueTracker.Shared;

namespace IssueTracker.Issues.API.REST.Version2.DataTransferObjects.Response;

/// <summary>
/// Issue Summary Page
/// </summary>
[SwaggerSchemaName("Issue Summary Page (v2)")]
public sealed class IssueSummaryPage
{
    /// <summary>
    /// Instantiates a new instance of the <see cref="IssueSummaryPage"/> class.
    /// </summary>
    public IssueSummaryPage(int pageNumber, int totalPages, int totalCount, IAsyncEnumerable<IssueSummaryDto> items)
    {
        PageNumber = pageNumber;
        TotalPages = totalPages;
        TotalCount = totalCount;
        Items = items;
    }

    /// <summary>
    /// Instantiates a new instance of the <see cref="IssueSummaryPage"/> class.
    /// </summary>
    internal IssueSummaryPage()
    {
        // for serialization
    }

    /// <summary>
    /// Convert <paramref name="page"/> to <see cref="IssueSummaryPage"/>
    /// </summary>
    public static IssueSummaryPage Convert(Domain.Services.Version2.DataTransferObjects.IssueSummaryPage page, IMapper mapper)
    {
        return new IssueSummaryPage(
            page.PageNumber,
            page.TotalPages,
            page.TotalCount,
            page.Items.Select(mapper.Map<IssueSummaryDto>));
    }

    /// <summary>
    /// Page Number
    /// </summary>
    /// <example>1</example>
    [Required]
    [Range(1, int.MaxValue)]
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Total number of pages
    /// </summary>
    /// <example>1</example>
    [Required]
    [Range(0, int.MaxValue)]
    public int TotalPages { get; set; } = 1;

    /// <summary>
    /// Total number of issues
    /// </summary>
    /// <example>10</example>
    [Required]
    [Range(0, int.MaxValue)]
    public int TotalCount { get; set; } = 0;

    /// <summary>
    /// Issues
    /// </summary>
    [Required]
    public IAsyncEnumerable<IssueSummaryDto> Items { get; set; } = AsyncEnumerable.Empty<IssueSummaryDto>();

}

