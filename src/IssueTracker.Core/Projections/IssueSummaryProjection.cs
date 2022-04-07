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

namespace IssueTracker.Core.Projections
{
    public sealed class IssueSummaryProjection : IEquatable<IssueSummaryProjection>
    {
        public IssueSummaryProjection(Guid id, string name)
        {
            Id = id;
            Title = name ?? string.Empty;
        }

        public Guid Id { get; }
        public string Title { get; }


        public IssueSummaryProjection With(Guid? id = null, string name = null)
        {
            return new IssueSummaryProjection(id ?? Id, name ?? Title);
        }

        public void Deconstruct(out Guid id, out string name)
        {
            id = Id;
            name = Title;
        }

        /// <inheritdoc />
        public bool Equals(IssueSummaryProjection other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Id.Equals(other.Id) && Title == other.Title;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) =>
            ReferenceEquals(this, obj) || obj is IssueSummaryProjection other && Equals(other);

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (Id.GetHashCode() * 397) ^ (Title != null ? Title.GetHashCode() : 0);
            }
        }
    }
}
