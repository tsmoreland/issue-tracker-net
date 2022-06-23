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

using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;

namespace IssueTracker.Issues.API.REST.Version2.DataTransferObjects.Request;

/// <summary>
/// Issue Link DTO used for patch requests
/// </summary>
public sealed class IssueLinkPatch
{
    private LinkType _linkType = LinkType.Related;
    private string _sourceId = string.Empty;
    private string _destinationId = string.Empty;

    [System.Text.Json.Serialization.JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    internal HashSet<string> ModifiedFields { get; } = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="IssueLinkPatch"/> class.
    /// </summary>
    public IssueLinkPatch()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IssueLinkPatch"/> class.
    /// copying values from <paramref name="issueLink"/>
    /// </summary>
    public static IssueLinkPatch FromDomainService(IssueLink issueLink)
    {
        return new IssueLinkPatch
        {
            _linkType = issueLink.Link,
            _sourceId = issueLink.LeftId.ToString(),
            _destinationId = issueLink.RightId.ToString(),
        };
    }

    /// <summary>
    /// Link Type
    /// </summary>
    /// <example>Related</example>
    public LinkType LinkType
    {
        get => _linkType;
        set
        {
            ModifiedFields.Add(nameof(LinkType));
            _linkType = value;
        }
    }

    /// <summary>
    /// Issue id for the link source
    /// </summary>
    /// <example>APP-1234</example>
    public string SourceId
    {
        get => _sourceId;
        set
        {
            ModifiedFields.Add(nameof(SourceId));
            _sourceId = value;
        }
    }

    /// <summary>
    /// Issue id for the link source destination
    /// </summary>
    /// <example>APP-1234</example>
    public string DestinationId
    {
        get => _destinationId;
        set
        {
            ModifiedFields.Add(nameof(DestinationId));
            _destinationId = value;
        }
    }
}
