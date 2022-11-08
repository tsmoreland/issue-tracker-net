//
// Copyright (c) 2022 Terry Moreland
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

namespace IssueTracker.Core.ValueObjects
{
    public sealed class User : ICloneable, IEquatable<User>
    {
        public User(Guid id, string fullName)
        {
            Id = id;
            FullName = fullName;
        }
        private User()
        {
            // required for ef core
        }


        public static User Unassigned { get; } = new User(Guid.Empty, "Unassigned");

        public Guid Id { get; private set; }
        public string FullName { get; private set; }

        public void Deconstruct(out Guid id, out string fullName)
        {
            id = Id;
            fullName = FullName;
        }

        /// <inheritdoc />
        public object Clone() => new User(Id, FullName);

        /// <inheritdoc />
        public bool Equals(User other)
        {
            return (other is not null && (ReferenceEquals(this, other) || (other.Id == Id && other.FullName == FullName)));
        }

        /// <inheritdoc />
        public override bool Equals(object obj) =>
            ReferenceEquals(this, obj) || obj is User other && Equals(other);

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (Id.GetHashCode() * 397) ^ (FullName != null ? FullName.GetHashCode() : 0);
            }
        }

    }
}
