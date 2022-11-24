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

using System.Diagnostics.CodeAnalysis;

namespace IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;

public readonly record struct IssueIdentifier(string Project, int IssueNumber) 
{
    public static IssueIdentifier Empty { get; } = new();

    public string Project { get; init; } = (Project is { Length: > 0 } and { Length: <= 3 })
        ? Project
        : throw new ArgumentException("Invalid project id");

    public int IssueNumber { get; init; } = (IssueNumber >= 0)
        ? IssueNumber
        : throw new ArgumentException("issue number must be a positive integer");

    public void Deconstruct(out string projectId, out int issueNumber)
    {
        projectId = Project;
        issueNumber = IssueNumber;
    }

    private static (string ProjectId, int Id) DeconstructId(string friendlyId)
    {
        int dashIndex = friendlyId.IndexOf('-');
        if (dashIndex == -1)
        {
            throw new ArgumentException("Malformed friendly id");
        }

        if (!int.TryParse(friendlyId[(dashIndex + 1)..], out int id))
        {
            throw new ArgumentException("Malformed friendly id, invalid id component");
        }

        return (friendlyId[..dashIndex], id);
    }

    /// <inheritdoc />
    public override string ToString() =>
        $"{Project}-{IssueNumber}";

    public static IssueIdentifier? FromStringIfNotNull(string? id)
    {
        if (id is null)
        {
            return null;
        }

        (string projectId, int issueNumber) = DeconstructId(id);
        return new IssueIdentifier(projectId, issueNumber);
    }
    public static IssueIdentifier FromString(string id)
    {
        (string projectId, int issueNumber) = DeconstructId(id);
        return new IssueIdentifier(projectId, issueNumber);
    }

    public static bool TryConvert(string id, [NotNullWhen(true)] out IssueIdentifier? issueId)
    {
        try
        {
            issueId = FromString(id);
            return true;
        }
        catch (ArgumentException)
        {
            issueId = null;
            return false;
        }

    }
}
