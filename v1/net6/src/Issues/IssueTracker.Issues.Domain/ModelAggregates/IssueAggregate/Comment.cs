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

using IssueTracker.Issues.Domain.DataContracts;

namespace IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;

public sealed class Comment : Entity, IEquatable<Comment>
{
    private readonly Issue _issue;

    /// <summary>
    /// Instantiates a new instance of the <see cref="Comment"/> class.
    /// </summary>
    /// <param name="issue"></param>
    /// <param name="author">comment author</param>
    /// <param name="content">comment content</param>
    /// <exception cref="ArgumentException">
    /// if <paramref name="content"/> is empty or more than <see cref="MaxContentLength"/> characters
    /// </exception>
    public Comment(Issue issue, CommentUser author, string content)
    {
        ArgumentNullException.ThrowIfNull(issue);
        ArgumentNullException.ThrowIfNull(author);

        if (content is not { Length: > 0 } or { Length: > MaxContentLength })
        {
            throw new ArgumentException($"Content cannot be empty or exceed {MaxContentLength} characters");
        }

        _issue = issue;
        IssueId = issue.Id;

        Author = author;
        Content = content;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    /// <inheritdoc cref="Comment(Issue, CommentUser, string)"/>
    public Comment(int id, Issue issue, CommentUser author, string content)
        : this(issue, author, content)
    {
        Id = id;
    }

    private Comment()
    {
        // used by entity framework
        Author = CommentUser.Anonymous;
        Content = string.Empty;
        _issue = null!;
        IssueId = string.Empty;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public const int MaxContentLength = 500;


    /// <summary>
    /// Unique Id for the comment
    /// </summary>
    public int Id { get; init; }

    public object IssueId { get; private set; }

    /// <summary>
    /// Comment Author
    /// </summary>
    public CommentUser Author { get; private set; }

    /// <summary>
    /// Comment Content
    /// </summary>
    public string Content { get; private set; }

    /// <summary>
    /// Creation Date
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; }

    /// <inheritdoc />
    public bool Equals(Comment? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Id == other.Id;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) =>
        ReferenceEquals(this, obj) || obj is Comment other && Equals(other);

    /// <inheritdoc />
    public override bool Equals(Entity? x, Entity? y)
    {
        return x is Comment commentX && y is Comment commentY && commentX.Equals(commentY);
    }

    /// <inheritdoc />
    public override int GetHashCode() => Id;

    /// <inheritdoc />
    public override int GetHashCode(Entity obj) => throw new NotImplementedException();
}
