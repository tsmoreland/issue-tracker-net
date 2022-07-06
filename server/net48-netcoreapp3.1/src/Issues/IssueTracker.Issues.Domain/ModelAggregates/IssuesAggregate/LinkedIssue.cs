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
using IssueTracker.Shared;

namespace IssueTracker.Issues.Domain.ModelAggregates.IssuesAggregate;

public sealed class LinkedIssue
{
    /// <summary>
    /// intended for use in seeding data
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public LinkedIssue(int id, LinkType linkType, int parentIssueId, int childIssueId)
    {
        Id = id;
        LinkType = linkType;
        ParentIssueId = parentIssueId;
        ChildIssueId = childIssueId;
    }
    /// <summary>
    /// intended for use in seeding data
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public LinkedIssue(LinkType linkType, int parentIssueId, int childIssueId)
        : this(0, linkType, parentIssueId, childIssueId)
    {
    }

    public LinkedIssue(LinkType linkType, Issue parentIssue, Issue childIssue)
    {
        ArgumentGuard.ThrowIfNull(parentIssue, nameof(parentIssue));
        ArgumentGuard.ThrowIfNull(childIssue, nameof(childIssue));

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

    public int Id { get; private set; }
    public LinkType LinkType { get; private set; } = LinkType.Related;

    public int ParentIssueId { get; private set; } 
    public Issue ParentIssue { get; private set; } 

    public int ChildIssueId { get; private set; } 
    public Issue ChildIssue { get; private set; } 

}
