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
/// Linked Summary
/// </summary>
[SwaggerSchemaName("Linked Issue Summary (v2)")]
public sealed class LinkedIssueSummaryDto
{
    /// <summary>
    /// Instantiates a new instance of the <see cref="LinkedIssueSummaryDto"/> class.
    /// </summary>
    public LinkedIssueSummaryDto(
        string id,
        string title,
        Priority priority,
        IssueType type,
        LinkType linkType)
    {
        Id = id;
        Title = title;
        Priority = priority;
        Type = type;
        LinkType = linkType;
    }

    /// <summary>
    /// Instantiates a new instance of the <see cref="LinkedIssueSummaryDto"/> class.
    /// </summary>
    internal LinkedIssueSummaryDto()
    {
        // for serialization
    }

    /// <summary>
    /// Issue Identifier
    /// </summary>
    /// <example>APP-1</example>
    [Required]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Issue Title
    /// </summary>
    /// <example>Sample title</example>
    [Required]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Issue Priority
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

    /// <summary>
    /// how this issue is related
    /// </summary>
    /// <example>Related</example>
    [Required]
    public LinkType LinkType { get; set; } = LinkType.Related;
}

