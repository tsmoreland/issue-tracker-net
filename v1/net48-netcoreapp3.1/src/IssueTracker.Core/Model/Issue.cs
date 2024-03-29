﻿//
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

using System;
using System.Collections.Generic;
using System.Linq;
using IssueTracker.Core.ValueObjects;
using IssueTracker.Core.Views;

namespace IssueTracker.Core.Model;

/// <summary>
/// Issue Entity
/// </summary>
public sealed class Issue
{
    /// <summary>
    /// Instanties a new instance of <see cref="Issue"/>
    /// </summary>
    public Issue()
        : this(0)
    {
    }

    /// <summary>
    /// Instanties a new instance of <see cref="Issue"/>
    /// </summary>
    public Issue(string title, string description, Priority priority)
        : this(title, description, priority, IssueType.Defect, User.Unassigned, User.Unassigned)
    {
    }

    /// <summary>
    /// Instanties a new instance of <see cref="Issue"/>
    /// </summary>
    public Issue(string title, string description, Priority priority, IssueType type, User assignee, User reporter)
        : this(0)
    {
        Title = title;
        Description = description;
        Priority = priority;
        Type = type;
        Assignee = assignee;
        Reporter = reporter;
        LastUpdated = DateTime.UtcNow;
    }

    /// <summary>
    /// Instanties a new instance of <see cref="Issue"/>, constructor provided primarily for use with entity framework
    /// </summary>
    public Issue(
        int id,
        string title,
        string description,
        Priority priority,
        IssueType type,
        User assignee,
        User reporter,
        IEnumerable<LinkedIssue> parentIssueEntities,
        IEnumerable<LinkedIssue> childIssueEntities,
        DateTime lastUpdated,
        string concurrencyToken)
        : this(id)
    {
        Title = title;
        Description = description;
        Priority = priority;
        Type = type;
        Assignee = assignee;
        Reporter = reporter;
        ParentIssueEntities = parentIssueEntities.ToList();
        ChildIssueEntities = childIssueEntities.ToList();

        LastUpdated = lastUpdated;
        ConcurrencyToken = concurrencyToken;
    }

    private Issue(int id)
    {
        Id = id;
    }

    /// <summary>
    /// Issue Sort Column
    /// </summary>
    public enum SortBy
    {
        /// <summary>
        /// Order by title alphabetically
        /// </summary>
        Title = 0,

        /// <summary>
        /// Order by <see cref="Model.Priority"/>
        /// </summary>
        Priority = 1,

        /// <summary>
        /// Order by <see cref="Model.IssueType"/>
        /// </summary>
        Type = 2,
    }

    /// <summary>
    /// Unique Id
    /// </summary>
    public int Id { get; private set; }

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
    /// Issue Type
    /// </summary>
    public IssueType Type { get; private set; } = IssueType.Defect;

    /// <summary>
    /// Assignee 
    /// </summary>
    public User Assignee { get; private set; } = User.Unassigned;

    /// <summary>
    /// Reporter
    /// </summary>
    public User Reporter { get; private set; } = User.Unassigned;

    /// <summary>
    /// Last Updated field used for auditing
    /// </summary>
    public DateTime LastUpdated { get; private set; } = DateTime.UtcNow;

    /// <summary>
    /// Concurrency token used to detected threading issues when persisting to the database
    /// </summary>
    public string ConcurrencyToken { get; private set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Issues that are linked to this one, this also serves as the ones that may be
    /// blocking this issue
    /// </summary>
    public IEnumerable<LinkedIssueView> ParentIssues =>
        ParentIssueEntities.Select(i => new LinkedIssueView(i.LinkType, i.ParentIssue));

    /// <summary>
    /// issues which this one links to, this also serves as issues that may be blocked by
    /// this issue
    /// </summary>
    public IEnumerable<LinkedIssueView> ChildIssues =>
        ChildIssueEntities.Select(i => new LinkedIssueView(i.LinkType, i.ChildIssue));

    public List<LinkedIssue> ParentIssueEntities { get; set; } = new List<LinkedIssue>();
    public List<LinkedIssue> ChildIssueEntities { get; set; } = new List<LinkedIssue>();

    /// <summary>
    /// adds link to child issue 
    /// </summary>
    /// <param name="linkType">the type of link</param>
    /// <param name="issue">the issue to link to</param>
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
    public void SetDescription(string value)
    {
        Description = !string.IsNullOrEmpty(value) 
            ? value.Trim()
            : string.Empty;
        LastUpdated = DateTime.UtcNow;
    }

    /// <summary>
    /// Update issue priority
    /// </summary>
    public void SetPriority(Priority priority)
    {
        Priority = priority;
        LastUpdated = DateTime.UtcNow;
    }

    /// <summary>
    /// Update issue type
    /// </summary>
    public void SetIssueType(IssueType type)
    {
        Type = type;
        LastUpdated = DateTime.UtcNow;
    }

    /// <summary>
    /// Upates top level properties with values from <paramref name="source"/>.
    /// Does not affect linked issues
    /// </summary>
    public void CopyTopLevelFrom(Issue source)
    {
        Title = source.Title;
        Description = source.Description;
        Priority = source.Priority;

        Assignee = (User)source.Assignee.Clone();
        Reporter = (User)source.Reporter.Clone();

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
