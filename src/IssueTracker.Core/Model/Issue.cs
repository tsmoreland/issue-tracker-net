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

namespace IssueTracker.Core.Model;

/// <summary>
/// Issue Entity
/// </summary>
/// <param name="Id"></param>
public sealed record class Issue(Guid Id)
{
    /// <summary>
    /// Instanties a new instance of <see cref="Issue"/>
    /// </summary>
    public Issue()
        : this(Guid.NewGuid())
    {
    }

    /// <summary>
    /// Instanties a new instance of <see cref="Issue"/>
    /// </summary>
    public Issue(string title, string description, Priority priority)
        : this(Guid.NewGuid())
    {
        Title = title;
        Description = description;
        Priority = priority;
        LastUpdated = DateTime.UtcNow;
    }
    /// <summary>
    /// Instanties a new instance of <see cref="Issue"/>, constructor provided primarily for use with entity framework
    /// </summary>
    public Issue(Guid id, string title, string description, Priority priority, DateTime lastUpdated, string? concurrencyToken, IEnumerable<LinkedIssue> parentIssueEntities, IEnumerable<LinkedIssue> childIssueEntities)
        : this(id)
    {
        Title = title;
        Description = description;
        Priority = priority;
        LastUpdated = lastUpdated;
        ConcurrencyToken = concurrencyToken;
        ParentIssueEntities = parentIssueEntities.ToList();
        ChildIssueEntities = childIssueEntities.ToList();
    }

    /// <summary>
    /// Issue Title
    /// </summary>
    public string Title { get; private set; } = string.Empty;
    /// <summary>
    /// Issue Description
    /// </summary>
    public string Description { get; private set; } = string.Empty;
    /// <summary>
    /// Issue Priority
    /// </summary>
    public Priority Priority { get; private set; } = Priority.Low;
    /// <summary>
    /// Last Updated field used for auditing
    /// </summary>
    public DateTime LastUpdated { get; private set; } = DateTime.UtcNow;

    /// <summary>
    /// Concurrency token used to detected threading issues when persisting to the database
    /// </summary>
    public string? ConcurrencyToken { get; private set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Issues that are linked to this one, this also serves as the ones that may be
    /// blocking this issue
    /// </summary>
    public IEnumerable<LinkedIssue> ParentIssues => ParentIssueEntities.AsEnumerable();

    /// <summary>
    /// issues which this one links to, this also serves as issues that may be blocked by
    /// this issue
    /// </summary>
    public IEnumerable<LinkedIssue> ChildIssues => ChildIssueEntities.AsEnumerable();

    public IList<LinkedIssue> ParentIssueEntities { get; init; } = new List<LinkedIssue>();
    public IList<LinkedIssue> ChildIssueEntities { get; init; } = new List<LinkedIssue>();

    public void LinkToIssue(LinkType linkType, Issue issue)
    {
        ChildIssueEntities.Add(new LinkedIssue(linkType, this, issue));
    }

    /// <summary>
    /// Update the issue name
    /// </summary>
    /// <param name="value">new name</param>
    /// <exception cref="ArgumentException">if name is empty or whitespace</exception>
    public void SetTitle(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Title cannot be empty");
        }
        Title = value;
        LastUpdated = DateTime.UtcNow;
    }
    /// <summary>
    /// Update issue description
    /// </summary>
    public void SetDescription(string? value)
    {
        Description = value is { Length: > 0 }
            ? value.Trim()
            : string.Empty;
        LastUpdated = DateTime.UtcNow;
    }

    /// <summary>
    /// Update issue priority
    /// </summary>
    public void ChangePriority(Priority priority)
    {
        Priority = priority;
        LastUpdated = DateTime.UtcNow;
    }
}
