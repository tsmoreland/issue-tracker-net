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

namespace IssueTracker.Core.Model;

public sealed class LinkedIssue
{
    /// <summary>
    /// intended for use in seeding data
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public LinkedIssue(Guid id, LinkType linkType, Guid parentIssueId, Guid childIssueId, string? concurrencyToken)
    {
        Id = id;
        LinkType = linkType;
        ParentIssueId = parentIssueId;
        ChildIssueId = childIssueId;
        ConcurrencyToken = concurrencyToken;
    }

    /// <summary>
    /// intended for use in seeding data
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public LinkedIssue(Guid id, LinkType linkType, Guid parentIssueId, Guid childIssueId)
        : this(id, linkType, parentIssueId, childIssueId, Guid.NewGuid().ToString())
    {
    }

    public LinkedIssue(LinkType linkType, Issue parentIssue, Issue childIssue)
        : this(Guid.NewGuid(), linkType, parentIssue, childIssue)
    {
        ArgumentNullException.ThrowIfNull(parentIssue, nameof(parentIssue));
        ArgumentNullException.ThrowIfNull(childIssue, nameof(childIssue));
    }

    public LinkedIssue(Guid id, LinkType linkType, Issue parentIssue, Issue childIssue)
    {
        ArgumentNullException.ThrowIfNull(parentIssue, nameof(parentIssue));
        ArgumentNullException.ThrowIfNull(childIssue, nameof(childIssue));

        Id = id;
        LinkType = linkType;
        ParentIssue = parentIssue;
        ParentIssueId = ParentIssue.Id;
        ChildIssue = childIssue;
        ChildIssueId = ChildIssue.Id;
    }

    private LinkedIssue()
    {
        // required by entity framework
    }

    public Guid Id { get; private set; } = Guid.NewGuid();

    public LinkType LinkType { get; init; } = LinkType.Related;

    public Guid ParentIssueId { get; init; } = Guid.Empty;
    public Issue ParentIssue { get; init; } = null!;

    public Guid ChildIssueId { get; init; } = Guid.Empty;
    public Issue ChildIssue { get; init; } = null!;

    /// <summary>
    /// Concurrency token used to detected threading issues when persisting to the database
    /// </summary>
    public string? ConcurrencyToken { get; private set; } = Guid.NewGuid().ToString();
}
