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

namespace IssueTracker.Shared;

/// <summary>
/// Storage class for caller location information
/// </summary>
/// <param name="CallerMemberName">method or property name</param>
/// <param name="CallerFilePath">source filename</param>
/// <param name="CallerLinesNumber">line number within <paramref name="CallerFilePath"/></param>
public readonly record struct CallerLocation(string CallerMemberName, string CallerFilePath, int CallerLinesNumber)
{
    private static readonly CallerLocation s_none = new(string.Empty, string.Empty, 0);
    public static ref readonly CallerLocation None => ref s_none;

    /// <inheritdoc />
    public override string ToString()
    {
        if (this == s_none)
        {
            return "(None)";
        }

        string callerFilePath = CallerFilePath;
        if (CallerFilePath is { Length: > 0 })
        {
            callerFilePath = Path.GetFileNameWithoutExtension(callerFilePath) ?? callerFilePath;
        }

        return $"{callerFilePath}.{CallerMemberName}:{CallerLinesNumber}";
    }
}
