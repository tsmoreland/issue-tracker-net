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

using System.ComponentModel.DataAnnotations;
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;
using IssueTracker.SwashbuckleExtensions.Abstractions;

namespace IssueTracker.Issues.API.Version2.REST.DataTransferObjects.Request;

/// <summary>
/// Model used to create a new Issue
/// </summary>
[SwaggerSchemaName("Add Issue")]
public sealed class AddIssueDto
{
    /// <summary>
    /// Instantiates a new instance of the <see cref="AddIssueDto"/> class.
    /// </summary>
    public AddIssueDto(string project, string title, string description, Priority priority, IssueType type, string? epicId)
    {
        Project = project;
        Title = title;
        Description = description;
        Priority = priority;
        Type = type;
        EpicId = epicId;
    }

    /// <summary>
    /// Instantiates a new instance of the <see cref="AddIssueDto"/> class.
    /// </summary>
    private AddIssueDto()
    {
        // used for serialization
    }

    /// <summary>
    /// Project Identifier
    /// </summary>
    /// <example>APP</example>
    [Required]
    public string Project { get; set; } = string.Empty;

    /// <summary>
    /// Issue Title
    /// </summary>
    /// <example>Sample Title</example>
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Issue description
    /// </summary>
    /// <example>Sample Description</example>
    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

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
    /// Epic Id
    /// </summary>
    /// <example>APP-1</example>
    [RegularExpression("[A-Z][A-Z][A-Z]-[0-9]+")]
    public string? EpicId { get; set; } 
}
