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
using System.Runtime.CompilerServices;
using IssueTracker.Core.Model;
using IssueTracker.Core.Projections;
using IssueTracker.Core.ValueObjects;
using IssueTracker.SwashbuckleExtensions.Abstractions;

namespace IssueTracker.RestApi.DataTransferObjects.Version2.Response;

/// <summary>
/// Short summary of an <see cref="Issue"/>
/// </summary>
[SwaggerSchemaName("Linked Issue Summary")]
public sealed class LinkedIssueSummaryDto
{
    /// <summary>
    /// Instantiates a new instance of he IssueSummaryDto Class
    /// </summary>
    public LinkedIssueSummaryDto(string id, string title, Priority priority, IssueType issueType, LinkType linkType)
    {
        Id = id;
        Title = title;
        Priority = priority;
        IssueType = issueType;
        LinkType = linkType;
    }

    /// <summary>
    /// Instantiates a new instance of he IssueSummaryDto Class
    /// </summary>
    public LinkedIssueSummaryDto()
    {
        Id = string.Empty;
        Title = string.Empty;
        Priority = Priority.Low;
        IssueType = IssueType.Defect;
    }

    /// <summary>
    /// Issue Id
    /// </summary>
    /// <example>APP-1234</example>
    [Required]
    public string Id { get; init; } 

    /// <summary>
    /// Issue Title
    /// </summary>
    /// <example>Example Title</example>
    [Required]
    public string Title { get; init; }

    /// <summary>
    /// Issue <see cref="Priority">Priority</see>   
    /// </summary>
    /// <value example="High">Issue <see cref="Priority">Priority</see></value>
    [Required]
    public Priority Priority { get; init; }

    /// <summary>
    /// Issue <see cref="Core.ValueObjects.IssueType">Type</see>
    /// </summary>
    /// <value example="Defect">Issue <see cref="IssueType">Type</see></value>
    [Required]
    public IssueType IssueType { get; init; }

    /// <summary>
    /// Issue <see cref="Core.ValueObjects.LinkType">Type</see>
    /// </summary>
    /// <value example="Defect">Issue <see cref="Core.ValueObjects.LinkType">Type</see></value>
    [Required]
    public LinkType LinkType { get; init; }

    /// <summary>
    /// Converts service DTO to versioned API DTO, for latest version of API the class is expected to be equivalent
    /// </summary>
    public static LinkedIssueSummaryDto FromProjection(LinkedIssueSummaryProjection model)
    {
        (IssueIdentifier id, string? title, Priority priority, IssueType issueType, LinkType linkType) = model;
        return new LinkedIssueSummaryDto(id.ToString(), title, priority, issueType, linkType);
    }

    /// <summary>
    /// Converts an asynchronous collection of <see cref="IssueSummaryProjection"/> to
    /// an asynchronous colleciton of <see cref="IssueSummaryDto"/>
    /// </summary>
    public static async IAsyncEnumerable<LinkedIssueSummaryDto> MapFrom(
        IAsyncEnumerable<LinkedIssueSummaryProjection> source, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (LinkedIssueSummaryProjection issueSummary in source.WithCancellation(cancellationToken))
        {
            yield return FromProjection(issueSummary);
        }
    }
}
