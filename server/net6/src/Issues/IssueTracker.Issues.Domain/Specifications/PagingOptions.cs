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

namespace IssueTracker.Issues.Domain.Specifications;

public sealed record class PagingOptions(int PageNumber, int PageSize)
{
    /// <summary>
    /// Page Number, starting at position 1
    /// </summary>
    public int PageNumber { get; init; } = ValidateOrThrow(PageNumber, nameof(PageNumber));

    /// <summary>
    /// Number of items in a page, must be greater than 0
    /// </summary>
    public int PageSize { get; init; } = ValidateOrThrow(PageSize, nameof(PageSize));


    /// <summary>
    /// Returns the number of items to skip 
    /// </summary>
    public int Skip => (PageNumber - 1) * PageSize;

    /// <summary>
    /// Snyonym of <see cref="PageSize"/>
    /// </summary>
    public int Take => PageSize;

    private static int ValidateOrThrow(int value, string pararmeterName)
    {
        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException(pararmeterName);
        }

        return value;
    }

}
