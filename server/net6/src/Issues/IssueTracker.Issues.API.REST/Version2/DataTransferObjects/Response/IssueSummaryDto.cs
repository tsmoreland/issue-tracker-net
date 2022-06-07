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

using System.ComponentModel.DataAnnotations;
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;
using IssueTracker.Shared;

namespace IssueTracker.Issues.API.REST.Version2.DataTransferObjects.Response;

/// <summary>
/// Issue Summary 
/// </summary>
[SwaggerSchemaName("Issue Summary (v2)")]
public sealed class IssueSummaryDto
{
    /// <summary>
    /// Instantiates a new instance of the <see cref="IssueSummaryDto"/> class.
    /// </summary>
    public IssueSummaryDto(string id, string title, Priority priority, IssueType type)
    {
        Id = id;
        Title = title;
        Priority = priority;
        Type = type;
    }

    /// <summary>
    /// Instantiates a new instance of the <see cref="IssueSummaryDto"/> class.
    /// </summary>
    public IssueSummaryDto()
    {
        
    }

    /// <summary>
    /// Unqiue Issue Id
    /// </summary>
    /// <example>APP-1</example>
    [Required]
    [RegularExpression("[A-Z][A-Z][A-Z]-[0-9]+")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Issue Title
    /// </summary>
    /// <example>Sample Title</example>
    [Required]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Priority
    /// </summary>
    /// <example>Low</example>
    [Required]
    public Priority Priority { get; set; } = Priority.Low;

    /// <summary>
    /// Issue Type
    /// </summary>
    /// <example>Defect</example>
    [Required]
    public IssueType Type { get; set; } = IssueType.Defect;
}

