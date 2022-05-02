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

using System.ComponentModel;
using IssueTracker.Core.Model;

namespace IssueTracker.Core.ValueObjects;

public sealed class LinkedIssue
{
    /// <summary>
    /// intended for use in seeding data
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public LinkedIssue(LinkType linkType, Guid parentIssueId, IssueIdentifier parentId, Guid childIssueId, IssueIdentifier childId, string? concurrencyToken)
    {
        LinkType = linkType;
        ParentId = parentId;
        ParentIssueId = parentIssueId;
        ChildId = childId;
        ChildIssueId = childIssueId;
        ConcurrencyToken = concurrencyToken;
    }

    /// <summary>
    /// intended for use in seeding data
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public LinkedIssue(LinkType linkType, Guid parentIssueId, IssueIdentifier parentId, Guid childIssueId, IssueIdentifier childId)
        : this(linkType, parentIssueId, parentId, childIssueId, childId, Guid.NewGuid().ToString())
    {
    }

    public LinkedIssue(LinkType linkType, Issue parentIssue, Issue childIssue)
    {
        ArgumentNullException.ThrowIfNull(parentIssue, nameof(parentIssue));
        ArgumentNullException.ThrowIfNull(childIssue, nameof(childIssue));

        LinkType = linkType;
        ParentIssue = parentIssue;
        ParentId = parentIssue.Id;
        ParentIssueId = parentIssue.IssueId;
        ChildIssue = childIssue;
        ChildId = childIssue.Id;
        ChildIssueId = ChildIssue.IssueId;
    }

    private LinkedIssue()
    {
        // required by entity framework
    }

    public LinkType LinkType { get; init; } = LinkType.Related;

    public IssueIdentifier ParentId { get; init; } 
    public Guid ParentIssueId { get; init; } 
    public Issue ParentIssue { get; init; } = null!;

    public IssueIdentifier ChildId { get; init; } 
    public Guid ChildIssueId { get; init; } 
    public Issue ChildIssue { get; init; } = null!;

    /// <summary>
    /// Concurrency token used to detected threading issues when persisting to the database
    /// </summary>
    public string? ConcurrencyToken { get; private set; } = Guid.NewGuid().ToString();
}
