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

using System.ComponentModel.DataAnnotations;
using IssueTracker.Shared;

namespace IssueTracker.Issues.API.REST.Version2.DataTransferObjects.Response;

/// <summary>
/// Triage User
/// </summary>
/// <param name="Id">Unique Identifier</param>
/// <param name="FullName">Full name</param>
[SwaggerSchemaName("Triage User")]
public sealed record class TriageUserDto(Guid Id, string FullName)
{
    internal TriageUserDto()
        : this(Guid.Empty, string.Empty)
    {
        // for serialization
    }

    /// <summary>
    /// Unique Identifier
    /// </summary>
    /// <example>F3296326-A40A-4208-AE6F-055EE106AC85</example>
    [Required]
    public Guid Id { get; } = Id;

    /// <summary>
    /// Full name
    /// </summary>
    /// <example>John Smith</example>
    [Required]
    public string FullName { get; } = FullName;

}

