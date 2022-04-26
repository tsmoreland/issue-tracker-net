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

namespace IssueTracker.RestApi.DataTransferObjects.Version2.QueryParameters;

/// <summary>
/// Sort Direction
/// </summary>
public enum SortDirection
{
    /// <summary>
    /// Ascending Order
    /// </summary>
    Ascending = 0,

    /// <summary>
    /// Descending Order
    /// </summary>
    Descending = 1,
}

/// <summary>
/// <see cref="SortDirection"/> extension methods
/// </summary>
public static class SortDirectionExtensions
{
    /// <summary>
    /// Convert <paramref name="value"/> to <see cref="Core.Model.SortDirection"/>
    /// </summary>
    public static Core.Model.SortDirection ToModel(this SortDirection value)
    {
        return (Core.Model.SortDirection)(int)value;
    }
}
