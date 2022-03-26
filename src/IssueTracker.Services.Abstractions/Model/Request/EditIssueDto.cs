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

namespace IssueTracker.Services.Abstractions.Model.Request;

/// <summary>
/// Model to use to update <see cref="Issue"/>
/// </summary>
public sealed class EditIssueDto
{
    /// <summary>
    /// Instantiates a new instance of the <see cref="EditIssueDto"/> class.
    /// </summary>
    public EditIssueDto(string title, string? description, Priority priority, IssueType? type)
    {
        Title = title;
        Description = description;
        Priority = priority;
        Type = type;
    }

    /// <summary>
    /// Issue Title
    /// </summary>
    /// <example>Example Title</example>
    public string Title { get; init; } 

    /// <summary>
    /// Issue Description
    /// </summary>
    /// <example>Example description</example>
    public string? Description { get; init; } 

    /// <summary>
    /// Issue Priority
    /// </summary>
    /// <example>High</example>
    public Priority Priority { get; init; }

    /// <summary>
    /// Issue Type
    /// </summary>
    public IssueType? Type { get; init; }

    /// <summary>
    /// Convert Data Transfer Object to <see cref="Issue"/>
    /// </summary>
    public Issue ToModel()
    {
        return new Issue(Title, Description ?? string.Empty, Priority, Type ?? IssueType.Defect);
    }
}
