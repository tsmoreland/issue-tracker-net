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

using System.Runtime.Serialization;

namespace IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.Exceptions;

/// <summary>
/// Issue thrown when an issue is not found
/// </summary>
[Serializable]
public sealed class IssueNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IssueNotFoundException"/> class.
    /// </summary>
    /// <param name="id"><inheritdoc cref="Id"/></param>
    public IssueNotFoundException(string id)
        : this(id, null, null)
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IssueNotFoundException"/> class
    /// with a specified error message.
    /// </summary>
    /// <param name="id"><inheritdoc cref="Id"/></param>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public IssueNotFoundException(string id, string? message)
        : this(id, message, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IssueNotFoundException"/> class
    /// with a specified error message and a reference to the inner
    /// exception that is the cause of this exception.
    /// </summary>
    /// <param name="id"><inheritdoc cref="Id"/></param>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">
    /// The exception that is the cause of the current exception,
    /// or a <see langword="null"/> reference (Nothing in Visual Basic) if no inner
    /// exception is specified.
    /// </param>
    public IssueNotFoundException(string id, string? message, Exception? innerException)
        : base(message, innerException)
    {
        Id = id;
    }

    private IssueNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        Id = info.GetString(nameof(Id)) ?? string.Empty;
    }

    /// <inheritdoc />
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(Id), Id, typeof(string));
    }

    /// <summary>
    /// Issue Display Id of the issue that wasn't found
    /// </summary>
    public string Id { get; }
}
