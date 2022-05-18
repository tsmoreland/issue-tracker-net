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
using IssueTracker.SwashbuckleExtensions.Abstractions;

namespace IssueTracker.Issues.API.Version2.REST.DataTransferObjects.Request;

/// <summary>
/// Model to use to update an Issue
/// </summary>
[SwaggerSchemaName("Edit Issue")]
public sealed class EditIssueDto
{
    /// <summary>
    /// Instantiates a new instance of the <see cref="EditIssueDto"/> class.
    /// </summary>
    public EditIssueDto(string title, string? description, Priority priority, IssueType type, string? epicId)
    {
        Title = title;
        Description = description;
        Priority = priority;
        Type = type;
        EpicId = epicId;
    }

    /// <summary>
    /// Instantiates a new instance of the <see cref="EditIssueDto"/> class.
    /// </summary>
    public EditIssueDto()
    {
        
    }

    /// <summary>
    /// Issue Title
    /// </summary>
    /// <example>Example Title</example>
    [Required]
    [MaxLength(200)]
    public string Title { get; init; } = string.Empty;

    /// <summary>
    /// Issue Description
    /// </summary>
    /// <example>Example description</example>
    [MaxLength(500)]
    public string? Description { get; init; }

    /// <summary>
    /// Issue Priority
    /// </summary>
    /// <example>High</example>
    [Required]
    public Priority Priority { get; init; } = Priority.Low;

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

    /// <summary>
    /// Deconstructor
    /// </summary>
    public void Deconstruct(out string? title, out string? description,
        out Priority? priority, out IssueType type, out string? epicId)
    {
        title = Title;
        description = Description;
        priority = Priority;
        type = Type;
        epicId = EpicId;
    }
}
