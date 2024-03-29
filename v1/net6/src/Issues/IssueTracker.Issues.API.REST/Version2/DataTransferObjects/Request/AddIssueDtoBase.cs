﻿//
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

using System.ComponentModel.DataAnnotations;
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;

namespace IssueTracker.Issues.API.REST.Version2.DataTransferObjects.Request;

/// <summary>
/// Add Issue DTO Base class, not intended for direct serialization/deserializtion
/// </summary>
public abstract class AddIssueDtoBase
{
    /// <summary>
    /// Instantiates a new instance of the <see cref="AddIssueDtoBase"/> class.
    /// </summary>
    protected AddIssueDtoBase(string project, string title, string description, Priority priority)
    {
        Project = project;
        Title = title;
        Description = description;
        Priority = priority;
    }

    /// <summary>
    /// Instantiates a new instance of the <see cref="AddIssueDtoBase"/> class.
    /// </summary>
    protected AddIssueDtoBase()
    {

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
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Issue Priority
    /// </summary>
    /// <example>Low</example>
    [Required]
    public Priority Priority { get; set; } = Priority.Low;

    /// <summary>
    /// Deconstructor
    /// </summary>
    public void Deconstruct(out string project, out string title,
        out string description, out Priority priority)
    {
        project = Project;
        title = Title;
        description = Description;
        priority = Priority;
    }
}
