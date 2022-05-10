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

using IssueTracker.Core.ValueObjects;
using IssueTracker.Core.Views;

namespace IssueTracker.Core.Model;

/// <summary>
/// Issue Entity
/// </summary>
public sealed record class Issue(Guid IssueId, IssueIdentifier Id)
{
    private ICollection<LinkedIssue> ParentIssueEntities { get; init; } = new HashSet<LinkedIssue>();
    private ICollection<LinkedIssue> ChildIssueEntities { get; init; } = new HashSet<LinkedIssue>();
    private readonly string _project = string.Empty;
    private readonly int _issueNumber;

    /// <summary>
    /// Instanties a new instance of <see cref="Issue"/>
    /// </summary>
    public Issue(string project, string title, string description, Priority priority)
        : this(project, title, description, priority, IssueType.Defect, TriageUser.Unassigned, Maintainer.Unassigned, null)
    {
    }

    /// <summary>
    /// Instanties a new instance of <see cref="Issue"/>
    /// </summary>
    public Issue(string project, string title, string description, Priority priority, IssueType type, TriageUser reporter, Maintainer assignee, Guid? epicIssueId)
        : this(project, title, description, priority, type, Array.Empty<LinkedIssue>(), Array.Empty<LinkedIssue>(),
            reporter, assignee, epicIssueId)
    {
    }

    /// <summary>
    /// Instanties a new instance of <see cref="Issue"/>, constructor provided primarily for use with entity framework
    /// </summary>
    public Issue(
        string project,
        string title,
        string description,
        Priority priority,
        IssueType type,
        IEnumerable<LinkedIssue> parentIssueEntities,
        IEnumerable<LinkedIssue> childIssueEntities,
        TriageUser reporter,
        Maintainer assignee,
        Guid? epicIssueId)
        : this(Guid.NewGuid(), new IssueIdentifier(project, 0))
    {
        _project = project;
        Title = title;
        Description = description;
        Priority = priority;
        Type = type;
        ParentIssueEntities = parentIssueEntities.ToHashSet();
        ChildIssueEntities = childIssueEntities.ToHashSet();
        Reporter = reporter;
        Assignee = assignee;
        EpicIssueId = epicIssueId;
    }

    /// <summary>
    /// Instanties a new instance of <see cref="Issue"/>, constructor provided primarily for use with entity framework
    /// </summary>
    public Issue(
        string project,
        int issueNumber,
        string title,
        string description,
        Priority priority,
        IssueType type,
        IEnumerable<LinkedIssue> parentIssueEntities,
        IEnumerable<LinkedIssue> childIssueEntities,
        TriageUser reporter,
        Maintainer assignee,
        Guid? epicIssueId,
        DateTime lastUpdated,
        string? concurrencyToken)
        : this(project, title, description, priority, type,
            parentIssueEntities, childIssueEntities,
            reporter, assignee, epicIssueId)
    {
        Id = new IssueIdentifier(project, issueNumber);
        EpicIssueId = epicIssueId;
        _issueNumber = issueNumber;
        LastUpdated = lastUpdated;
        ConcurrencyToken = concurrencyToken;
    }

    /// <summary>
    /// Instanties a new instance of <see cref="Issue"/>
    /// </summary>
    private Issue()
        : this(Guid.NewGuid(), IssueIdentifier.Empty)
    {
        // Used by entity framework
        _project = Id.Project;
        _issueNumber = Id.IssueNumber;
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
        /// Order by <see cref="ValueObjects.Priority"/>
        /// </summary>
        Priority = 1,

        /// <summary>
        /// Order by <see cref="IssueType"/>
        /// </summary>
        Type = 2,
    }

    /// <summary>
    /// Database Id
    /// </summary>
    public Guid IssueId { get; init; } = IssueId;

    /// <summary>
    /// Issue Identifier
    /// </summary>
    public IssueIdentifier Id { get; init; } = Id;


    /// <summary>
    /// unique identifier of the owning project
    /// </summary>
    public string Project
    {
        get => _project;
        init
        {
            if (value is not { Length: > 0 } and { Length: <= 3 })
            {
                throw new ArgumentException("project id must be between 1 and 3 characters long");
            }
            _project = value;
            Id = new IssueIdentifier(value, IssueNumber);
        }
    } 

    /// <summary>
    /// Unique identifier within the owning project
    /// </summary>
    public int IssueNumber
    {
        get => _issueNumber;
        init
        {
            if (value < 0)
            {
                throw new ArgumentException("Issue number cannot be negative");
            }

            _issueNumber = value;
            Id = new IssueIdentifier(Project, value);
        }
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
    /// Issue Type
    /// </summary>
    public IssueType Type { get; private set; } = IssueType.Defect;

    /// <summary>
    /// User assigned to address the issue
    /// </summary>
    public Maintainer Assignee { get; private set; } = Maintainer.Unassigned;

    /// <summary>
    /// User that reported the issue
    /// </summary>
    public TriageUser Reporter { get; private set; } = TriageUser.Unassigned;

    /// <summary>
    /// reference to Epic
    /// </summary>
    public Guid? EpicIssueId { get; private set; } 

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

    /// <summary>
    /// Last Updated field used for auditing
    /// </summary>
    public DateTime LastUpdated { get; private set; } = DateTime.UtcNow;

    /// <summary>
    /// Concurrency token used to detected threading issues when persisting to the database
    /// </summary>
    public string? ConcurrencyToken { get; private set; } = Guid.NewGuid().ToString();

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
    }

    /// <summary>
    /// Update issue description
    /// </summary>
    public void SetDescription(string? value)
    {
        Description = value is { Length: > 0 }
            ? value.Trim()
            : string.Empty;
    }

    /// <summary>
    /// Update issue priority
    /// </summary>
    public void SetPriority(Priority priority)
    {
        Priority = priority;
    }

    /// <summary>
    /// Update issue type
    /// </summary>
    public void SetIssueType(IssueType type)
    {
        if (type is IssueType.Epic)
        {
            RemoveEpic();
        }
        Type = type;
    }

    /// <summary>
    /// Update issue priority
    /// </summary>
    public void ChangePriority(Priority priority)
    {
        Priority = priority;
    }

    /// <summary>
    /// Sets the epic issue to <paramref name="epic"/> 
    /// </summary>
    /// <param name="epic">the epic to be used by this instance</param>
    /// <exception cref="InvalidOperationException">
    /// if <see cref="Type"/> is <see cref="IssueType.Epic"/>; because
    /// an epic cannot have an epic
    /// </exception>
    public void SetEpic(Issue epic)
    {
        if (Type is IssueType.Epic)
        {
            throw new InvalidOperationException("Cannot set epic on an issue with type Epic");
        }

        EpicIssueId = epic.IssueId;
    }

    /// <summary>
    /// Removes the associated Epic 
    /// </summary>
    public void RemoveEpic()
    {
        EpicIssueId = null;
    }

    /// <summary>
    /// Update Assignee value to <paramref name="assignee"/>
    /// </summary>
    public void SetAssignee(Maintainer assignee)
    {
        ArgumentNullException.ThrowIfNull(assignee, nameof(assignee));

        Assignee = assignee;
    }

    /// <summary>
    /// Update Reporter value to <paramref name="reporter"/>
    /// </summary>
    public void SetReporter(TriageUser reporter)
    {
        ArgumentNullException.ThrowIfNull(reporter, nameof(reporter));
        Reporter = reporter;
    }

    public void RefreshLastUpdated()
    {
        LastUpdated = DateTime.UtcNow;
    }
}
