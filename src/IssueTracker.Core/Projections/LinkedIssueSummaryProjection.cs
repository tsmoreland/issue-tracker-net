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

using System;
using IssueTracker.Core.Model;

namespace IssueTracker.Core.Projections
{
    public sealed class LinkedIssueSummaryProjection : IEquatable<LinkedIssueSummaryProjection>
    {
        public LinkedIssueSummaryProjection(Guid id, string name, Priority priority, IssueType issueType, LinkType linkType)
        {
            Id = id;
            Priority = priority;
            IssueType = issueType;
            LinkType = linkType;
            Title = name ?? string.Empty;
        }

        public Guid Id { get; }
        public string Title { get; }
        public Priority Priority { get; }
        public IssueType IssueType { get; }
        public LinkType LinkType { get; }


        public LinkedIssueSummaryProjection With(Guid? id = null, string name = null, Priority? priority = null, IssueType? issueType = null, LinkType? linkType = null)
        {
            return new LinkedIssueSummaryProjection(id ?? Id, name ?? Title, priority ?? Priority, issueType ?? IssueType, linkType ?? LinkType);
        }

        public void Deconstruct(out Guid id, out string name, out Priority priority, out IssueType issueType, out LinkType linkType)
        {
            id = Id;
            name = Title;
            priority = Priority;
            issueType = IssueType;
            linkType = LinkType;
        }

        /// <inheritdoc />
        public bool Equals(LinkedIssueSummaryProjection other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Id.Equals(other.Id) && LinkType == other.LinkType;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) =>
            ReferenceEquals(this, obj) || obj is LinkedIssueSummaryProjection other && Equals(other);

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (Id.GetHashCode() * 397) ^ (LinkType.GetHashCode() * 397);
            }
        }
    }
}
