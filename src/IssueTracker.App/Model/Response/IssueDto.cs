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
using IssueTracker.App.Attributes;
using IssueTracker.Core.Model;

namespace IssueTracker.App.Model.Response;

/// <summary>
/// Issue Details
/// </summary>
[SwaggerSchemaName("Issue Details")]
public sealed class IssueDto
{
    /// <summary>
    /// Instantiates a new instance of the <see cref="IssueDto"/> class.
    /// </summary>
    public IssueDto(Guid id, string title, string? description, Priority priority)
    {
        Id = id;
        Title = title;
        Description = description;
        Priority = priority;
    }

    /// <summary>
    /// Issue Id
    /// </summary>
    /// <example>48EE3BF9-C81D-4FE4-AB02-220C0122AFE4</example>
    [Required]
    public Guid Id { get; init; }

    /// <summary>
    /// Issue Title
    /// </summary>
    /// <example>Example Title</example>
    [Required]
    public string Title { get; init; } 

    /// <summary>
    /// Issue Description
    /// </summary>
    /// <example>Example Description</example>
    [Required]
    public string? Description { get; init; }

    /// <summary>
    /// Issue Priority
    /// </summary>
    /// <example>Medium</example>
    [Required]
    public Priority Priority { get; init; } 

    /// <summary>
    /// Converts <see cref="Issue"/> to <see cref="IssueDto"/>
    /// </summary>
    public static IssueDto FromIssue(Issue issue)
    {
        return new IssueDto(issue.Id, issue.Title, issue.Description, issue.Priority);
    }
}
