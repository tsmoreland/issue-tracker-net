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
using IssueTracker.Core.Projections;

namespace IssueTracker.RestApi.Shared.Model.Response;

public class IssueSummaryV1
{
    /// <summary>
    /// Instantiates a new instance of he IssueSummaryDto Class
    /// </summary>
    public IssueSummaryV1(int id, string title)
    {
        Id = id;
        Title = title;
    }

    /// <summary>
    /// Instantiates a new instance of he IssueSummaryDto Class
    /// </summary>
    public IssueSummaryV1()
    {
        Id = 0;
        Title = string.Empty;

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
    /// Converts service DTO to versioned API DTO, for latest version of API the class is expected to be equivalent
    /// </summary>
    public static IssueSummaryV1 FromProjection(IssueSummaryProjection model)
    {
        (int id, string title, _, _) = model;
        return new IssueSummaryV1(id, title);
    }

    /// <summary>
    /// Converts an asynchronous collection of <see cref="IssueSummaryProjection"/> to
    /// an asynchronous colleciton of <see cref="IssueSummaryDto"/>
    /// </summary>
    public static async Task<IReadOnlyList<IssueSummaryV1>> MapFrom(
        Task<IReadOnlyList<IssueSummaryProjection>> sourceTask, CancellationToken cancellationToken)
    {
        IReadOnlyList<IssueSummaryProjection> source = await sourceTask;
        cancellationToken.ThrowIfCancellationRequested();
        return source.Select(FromProjection).ToList();
    }
}
