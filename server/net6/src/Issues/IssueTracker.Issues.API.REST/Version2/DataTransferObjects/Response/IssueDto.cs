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
using IssueTracker.Shared;

namespace IssueTracker.Issues.API.REST.Version2.DataTransferObjects.Response;

/// <summary>
/// Issue Response DTO
/// </summary>
[SwaggerSchemaName("Issue (v2)")]
public sealed record class IssueDto(string Id, string Title, string Description,
        Priority Priority, IssueType Type, IssueStateValue State,
        TriageUserDto Reporter, MaintainerDto Assignee,
        string? EpicId)
{
    private IssueDto()
        : this(string.Empty,
            string.Empty,
            string.Empty,
            default,
            default,
            default,
            new TriageUserDto(),
            new MaintainerDto(),
            null)
{
    // for serialization
}


/// <summary>
/// Unique issue id
/// </summary>
/// <example>APP-1</example>
[Required]
[RegularExpression("[A-Z][A-Z][A-Z]-[0-9]+")]
public string Id { get; set; } = Id;

/// <summary>
/// Issue Title
/// </summary>
/// <example>Sample Title</example>
[Required]
public string Title { get; set; } = Title;

/// <summary>
/// Short description of the issue
/// </summary>
/// <example>Sample Description</example>
[Required]
public string? Description { get; set; } = Description;

/// <summary>
/// Issue Priority
/// </summary>
/// <example>Low</example>
[Required]
public Priority Priority { get; set; } = Priority;

/// <summary>
/// Issue type
/// </summary>
/// <example>Defect</example>
[Required]
public IssueType Type { get; set; } = Type;

/// <summary>
/// Issue State
/// </summary>
/// <example>BackLog</example>
[Required]
public IssueStateValue State { get; set; } = State;

/// <summary>
/// Reporter
/// </summary>
[Required]
public TriageUserDto Reporter { get; set; } = Reporter;

/// <summary>
/// Assigned maintainer
/// </summary>
[Required]
public MaintainerDto Assignee { get; set; } = Assignee;

/// <summary>
/// Epic Id
/// </summary>
/// <example>APP-1</example>
[RegularExpression("[A-Z][A-Z][A-Z]-[0-9]+")]
public string? EpicId { get; set; } = EpicId;

}

