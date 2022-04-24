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
using IssueTracker.Core.Model;
using IssueTracker.SwashbuckleExtensions.Abstractions;

namespace IssueTracker.RestApi.DataTransferObjects.Version2.Request;

/// <summary>
/// Model to use to update <see cref="Issue"/>
/// </summary>
[SwaggerSchemaName("Edit Issue")]
public sealed class EditIssueDto
{
    /// <summary>
    /// Instantiates a new instance of the <see cref="EditIssueDto"/> class.
    /// </summary>
    public EditIssueDto(string title, string? description, Priority priority, IssueType type)
    {
        Title = title;
        Description = description;
        Priority = priority;
        Type = type;
    }

    /// <summary>
    /// Instantiates a new instance of the <see cref="EditIssueDto"/> class.
    /// </summary>
    public EditIssueDto()
    {
        // required for serialization
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
    [Required]
    public IssueType Type { get; init; } = IssueType.Defect;

    /// <summary>
    /// Update exisitng issue with provided values
    /// </summary>
    /// <param name="issue">The issue to update</param>
    public void Update(Issue issue)
    {
        issue.SetTitle(Title);
        issue.SetDescription(Description);
        issue.SetPriority(Priority);
        issue.SetIssueType(Type);
    }
}
