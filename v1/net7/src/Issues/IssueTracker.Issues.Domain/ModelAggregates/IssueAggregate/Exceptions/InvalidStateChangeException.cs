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
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.Commands;

namespace IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.Exceptions;

/// <summary>
/// Exception thrown if an invalid <see cref="StateChangeCommand"/> is executed
/// </summary>
[Serializable]
public sealed class InvalidStateChangeException : Exception
{

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidStateChangeException"/> class
    /// </summary>
    /// <param name="state"><inheritdoc cref="State"/></param>
    /// <param name="commandType"><inheritdoc cref="CommandType"/></param>
    public InvalidStateChangeException(IssueStateValue state, Type commandType)
        : this(state, commandType, null, null)
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidStateChangeException"/> class
    /// with a specified error message.
    /// </summary>
    /// <param name="state"><inheritdoc cref="State"/></param>
    /// <param name="commandType"><inheritdoc cref="CommandType"/></param>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public InvalidStateChangeException(IssueStateValue state, Type commandType, string? message)
        : this(state, commandType, message, null)
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidStateChangeException"/> class
    /// with a specified error message and a reference to the inner
    /// exception that is the cause of this exception.
    /// </summary>
    /// <param name="state"><inheritdoc cref="State"/></param>
    /// <param name="commandType"><inheritdoc cref="CommandType"/></param>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">
    /// The exception that is the cause of the current exception,
    /// or a <see langword="null"/> reference (Nothing in Visual Basic) if no inner
    /// exception is specified.
    /// </param>
    public InvalidStateChangeException(IssueStateValue state, Type commandType, string? message, Exception? innerException)
        : base(message, innerException)
    {
        State = state;
        CommandType = commandType;
    }

    private InvalidStateChangeException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        State = (IssueStateValue)info.GetInt32(nameof(State));

        string type = info.GetString(nameof(CommandType)) ?? typeof(StateChangeCommand).AssemblyQualifiedName ?? string.Empty;
        CommandType = Type.GetType(type) ?? typeof(StateChangeCommand);
    }

    /// <inheritdoc />
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);

        info.AddValue(nameof(State), (int)State);
        info.AddValue(nameof(CommandType), CommandType.AssemblyQualifiedName);
    }

    /// <summary>
    /// the current state of the issue
    /// </summary>
    public IssueStateValue State { get; }

    /// <summary>
    /// the type of command attempted
    /// </summary>
    public Type CommandType { get; }

}
