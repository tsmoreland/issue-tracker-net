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
using IssueTracker.Core.Projections;

namespace IssueTracker.RestApi.Shared.Model.Response;

public class IssueSummaryV2
{
    /// <summary>
    /// Instantiates a new instance of he IssueSummaryDto Class
    /// </summary>
    public IssueSummaryV2(int id, string title, Priority priority, IssueType type)
    {
        Id = id;
        Title = title;
        Priority = priority;
        Type = type;
    }

    /// <summary>
    /// Instantiates a new instance of he IssueSummaryDto Class
    /// </summary>
    public IssueSummaryV2()
    {
        Id = 0;
        Title = string.Empty;
        Priority = Priority.Low;
        Type = IssueType.Defect;
    }

    /// <summary>
    /// Issue Id
    /// </summary>
    /// <example>3C1152EC-DC0C-4AB0-8AF9-10DE5A9705D5</example>
    [Required]
    public int Id { get; set; } 

    /// <summary>
    /// Issue Title
    /// </summary>
    /// <example>Example Title</example>
    [Required]
    public string Title { get; set; }

    /// <summary>
    /// Issue <see cref="Priority">Priority</see>   
    /// </summary>
    /// <value example="High">Issue <see cref="Priority">Priority</see></value>
    [Required]
    public Priority Priority { get; set; }

    /// <summary>
    /// Issue <see cref="IssueType">Type</see>
    /// </summary>
    /// <value example="Defect">Issue <see cref="IssueType">Type</see></value>
    [Required]
    public IssueType Type { get; set; }

    /// <summary>
    /// Converts service DTO to versioned API DTO, for latest version of API the class is expected to be equivalent
    /// </summary>
    public static IssueSummaryV2 FromProjection(IssueSummaryProjection model)
    {
        (int id, string? title, Priority priority, IssueType type) = model;
        return new IssueSummaryV2(id, title, priority, type);
    }

    /// <summary>
    /// Converts an asynchronous collection of <see cref="IssueSummaryProjection"/> to
    /// an asynchronous colleciton of <see cref="IssueSummaryV2"/>
    /// </summary>
    public static async Task<IReadOnlyList<IssueSummaryV2>> MapFrom(
        Task<IReadOnlyList<IssueSummaryProjection>> sourceTask, CancellationToken cancellationToken)
    {
        IReadOnlyList<IssueSummaryProjection> source = await sourceTask;
        cancellationToken.ThrowIfCancellationRequested();
        return source.Select(FromProjection).ToList();
    }
}
