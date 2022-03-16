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

namespace IssueTracker.Services.Abstractions.Model.Response;

/// <summary>
/// Short summary of an <see cref="Issue"/>
/// </summary>
[SwaggerSchemaName("Issue Summary")]
public sealed class IssueSummaryDto
{
    public IssueSummaryDto(Guid id, string title)
    {
        Id = id;
        Title = title;
    }

    public IssueSummaryDto()
    {
        Id = Guid.Empty;
        Title = string.Empty;

    }

    /// <summary>
    /// Issue Id
    /// </summary>
    /// <example>3C1152EC-DC0C-4AB0-8AF9-10DE5A9705D5</example>
    [Required]
    public Guid Id { get; init; } 

    /// <summary>
    /// Issue Title
    /// </summary>
    /// <example>Example Title</example>
    [Required]
    public string Title { get; init; }
}
