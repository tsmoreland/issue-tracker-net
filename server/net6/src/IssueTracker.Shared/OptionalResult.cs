﻿//
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

using System.Diagnostics.CodeAnalysis;

namespace IssueTracker.Shared;

/// <summary>
/// Factory methods for <see cref="OptionalResult{T}"/>
/// </summary>
public static class OptionalResult
{
    /// <summary>
    /// Creates a new instance of <see cref="OptionalResult{T}"/> storing <paramref name="value"/>
    /// </summary>
    /// <typeparam name="T">The value to be stored <see cref="OptionalResult{T}"/></typeparam>
    /// <param name="value">The value to be stored in the <see cref="OptionalResult{T}"/></param>
    /// <returns>A new instance of <see cref="OptionalResult{T}"/></returns>
    public static OptionalResult<T> Ok<T>(T value) =>
        new(true, value, null);

    /// <summary>
    /// Creates a new instance of the <see cref="OptionalResult{T}"/> class which
    /// does not contain a value, with <paramref name="error"/> as the reason why
    /// </summary>
    /// <typeparam name="T">The value that would be stored in <see cref="OptionalResult{T}"/></typeparam>
    /// <param name="error">The reason why there is no value present</param>
    /// <returns>A new instance of <see cref="OptionalResult{T}"/></returns>
    public static OptionalResult<T> Failure<T>(Exception? error) =>
        new(false, default, error);
}

/// <summary>
/// Represents a result that may or may not exist
/// </summary>
/// <typeparam name="T">The result value if present</typeparam>
public readonly struct OptionalResult<T>
{
    private readonly Exception? _error;

    /// <summary>
    /// Instantiates a new instance of the <see cref="OptionalResult{T}"/> class
    /// </summary>
    /// <param name="hasValue"><see langword="true"/> if <see cref="HasValue"/></param>
    /// <param name="value">value</param>
    /// <param name="error">
    /// error that represents why <paramref name="hasValue"/>
    /// is <see langword="false"/>
    /// </param>
    internal OptionalResult(bool hasValue, T? value, Exception? error)
    {
        HasValue = hasValue;
        Value = value;
        _error = error;
    }

    /// <summary>
    /// Returns <see langword="true"/>
    /// </summary>
    [MemberNotNullWhen(true, nameof(Value))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool HasValue { get; }

    /// <summary>
    /// Result value if <see cref="HasValue"/>
    /// </summary>
    public T? Value { get; }

    /// <summary>
    /// Returns an <see cref="Exception"/> which represents why
    /// the value <see cref="HasValue"/> is <see langword="false"/>
    /// </summary>
    public Exception? Error => HasValue
        ? null
        : _error ?? new InvalidOperationException();

    /// <summary>
    /// Returns <see cref="Value"/> if present or throws <see cref="Error"/>
    /// </summary>
    /// <returns><see cref="Value"/> if present</returns>
    public readonly T OrThrow() => HasValue
        ? Value
        : throw Error;

    /// <summary>
    /// Returns <see cref="Value"/> if <see cref="HasValue"/> or <paramref name="else"/>
    /// </summary>
    /// <param name="else">alternate return value if <see cref="HasValue"/> is <see langword="false"/></param>
    /// <returns>
    /// <see cref="Value"/> if <see cref="HasValue"/> or <paramref name="else"/>
    /// </returns>
    public readonly T OrElse(in T @else) => HasValue
        ? Value
        : @else;

    /// <summary>
    /// Uses <paramref name="map"/> to convert <see cref="Value"/> if <see cref="HasValue"/>
    /// otherwise returns <paramref name="else"/>
    /// </summary>
    /// <typeparam name="TMapped">The type to convert to</typeparam>
    /// <param name="map">converter function</param>
    /// <param name="else">alternative return value if <see cref="HasValue"/> is <see langword="false"/>></param>
    /// <returns>Mapped value if <see cref="HasValue"/> otherwise <paramref name="else"/></returns>
    public readonly TMapped MapOrElse<TMapped>(Func<T, TMapped> map, TMapped @else)
    {
        ArgumentNullException.ThrowIfNull(map, nameof(map));
        return HasValue
            ? map(Value)
            : @else;
    }

    /// <summary>
    /// Uses <paramref name="map"/> to convert <see cref="Value"/> if <see cref="HasValue"/>
    /// otherwise throws <see cref="Error"/>
    /// </summary>
    /// <typeparam name="TMapped">The type to convert to</typeparam>
    /// <param name="map">converter function</param>
    /// <returns>Mapped value if <see cref="HasValue"/></returns> 
    public readonly TMapped MapOrThrow<TMapped>(Func<T, TMapped> map)
    {
        ArgumentNullException.ThrowIfNull(map, nameof(map));
        return HasValue
            ? map(Value)
            : throw Error;
    }
}
