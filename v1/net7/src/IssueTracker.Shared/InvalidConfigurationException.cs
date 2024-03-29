﻿//
// Copyright (c) 2023 Terry Moreland
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

namespace IssueTracker.Shared;

/// <summary>
/// Represents invalid application configuration such as an entry missing from appsettings.json
/// </summary>
public sealed class InvalidConfigurationException : Exception
{
    /// <summary>
    /// Instantiates a new instance of the <see cref="InvalidConfigurationException"/> class.
    /// </summary>
    public InvalidConfigurationException()
        : this(null, null)
    {

    }

    /// <summary>
    /// Instantiates a new instance of the <see cref="InvalidConfigurationException"/> class with
    /// a specified error <paramref name="message"/>
    /// </summary>
    /// <param name="message">summary of the error</param>
    public InvalidConfigurationException(string? message)
        : this(message, null)
    {
    }

    /// <summary>
    /// Instantiates a new instance of the <see cref="InvalidConfigurationException"/> class with
    /// a specified error <paramref name="message"/> and a reference to the underlying cause
    /// </summary>
    /// <param name="message">summary of the error</param>
    /// <param name="innerException">underlying cause of the exception</param>
    public InvalidConfigurationException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
