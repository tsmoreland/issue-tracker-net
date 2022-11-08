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

using System.ComponentModel.DataAnnotations;
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;
using IssueTracker.Shared;

namespace IssueTracker.Issues.API.REST.Version2.DataTransferObjects.Request;

/// <summary>
/// Model use to create a Task issue
/// </summary>
[SwaggerSchemaName("Add Task Issue")]
public sealed class AddTaskIssueDto : AddIssueDtoBase
{
    /// <summary>
    /// Instantiates a new instance of the <see cref="AddTaskIssueDto"/> class.
    /// </summary>
    public AddTaskIssueDto(string project, string title, string description, Priority priority, string? epicId)
    {
        Project = project;
        Title = title;
        Description = description;
        Priority = priority;
        EpicId = epicId;
    }

    /// <summary>
    /// Instantiates a new instance of the <see cref="AddTaskIssueDto"/> class.
    /// </summary>
    public AddTaskIssueDto()
    {

    }

    /// <summary>
    /// Issue Type
    /// </summary>
    public static IssueType Type => IssueType.Task;

    /// <summary>
    /// Epic Id
    /// </summary>
    /// <example>APP-1</example>
    [RegularExpression("[A-Z][A-Z][A-Z]-[0-9]+")]
    public string? EpicId { get; set; }
}
