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

using IssueTracker.Core.Model;

namespace IssueTracker.Core.Views;

public sealed class LinkedIssueView : IEquatable<LinkedIssueView>
{
    private readonly Issue _issue;

    public LinkedIssueView(LinkType linkType, Issue issue)
    {
        _issue = issue;
        LinkType = linkType;
    }

    public Guid Id => _issue.Id;
    public string Title => _issue.Title;
    public string Description => _issue.Description;
    public Priority Priority => _issue.Priority;
    public IssueType Type => _issue.Type;
    public LinkType LinkType { get; init; }
    public DateTime LastUpdated => _issue.LastUpdated;
    public IEnumerable<LinkedIssueView> Parents => _issue.ParentIssues;
    public IEnumerable<LinkedIssueView> Children => _issue.ChildIssues;


    public void Deconstruct(out LinkType linkType,
        out Guid id,
        out string title,
        out string? description,
        out Priority priority,
        out IssueType type,
        out DateTime? lastUpdated,
        out IEnumerable<LinkedIssueView> parents,
        out IEnumerable<LinkedIssueView> children)
    {
        linkType = LinkType;
        id = Id;
        title = Title;
        description = Description;
        priority = Priority;
        type = Type;
        lastUpdated = LastUpdated;
        parents = Parents;
        children = Children;
    }

    /// <inheritdoc />
    public bool Equals(LinkedIssueView? other)
    {
        return other is not null &&
               (ReferenceEquals(this, other) || (_issue.Equals(other._issue) && LinkType == other.LinkType));
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) =>
        ReferenceEquals(this, obj) || obj is LinkedIssueView other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() =>
        HashCode.Combine(_issue, (int) LinkType);
}
