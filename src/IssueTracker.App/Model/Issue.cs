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

using System.Diagnostics.CodeAnalysis;

namespace IssueTracker.App.Model;

public sealed record class Issue(Guid Id)
{
    public Issue()
        : this(Guid.NewGuid())
    {
    }

    public Issue(string name, string description, Priority priority)
        : this(Guid.NewGuid())
    {
        Name = name;
        Description = description;
        Priority = priority;
        LastUpdated = DateTime.UtcNow;
    }
    public Issue(Guid id, string name, string description, Priority priority, DateTime lastUpdated, string? concurrencyToken)
        : this(id)
    {
        Name = name;
        Description = description;
        Priority = priority;
        LastUpdated = lastUpdated;
        ConcurrencyToken = concurrencyToken;
    }

    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public Priority Priority { get; private set; } = Priority.Low;
    public DateTime LastUpdated { get; private set; } = DateTime.UtcNow;


    [SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Used by EF", Scope = "member", Target = "~P:IssueTracker.App.Model.Issue.ConcurrencyToken")]
    private string? ConcurrencyToken { get; set; } = string.Empty;

    public void SetName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Name cannot be empty");
        }
        Name = value;
        LastUpdated = DateTime.UtcNow;
    }
    public void SetDescription(string value)
    {
        Description = value is { Length: > 0 }
            ? value.Trim()
            : string.Empty;
        LastUpdated = DateTime.UtcNow;
    }

    public void ChangePriority(Priority priority)
    {
        Priority = priority;
        LastUpdated = DateTime.UtcNow;
    }
}
