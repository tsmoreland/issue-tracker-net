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

using IssueTracker.Issues.Domain.DataContracts;
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.State;

namespace IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;

public sealed class Issue : Entity
{
    private string _title = string.Empty;
    private string _description = string.Empty;
    private IssueIdentifier? _epicId;
    private string _project = string.Empty;
    private int _issueNumber;
    private IssueType _type = IssueType.Defect;
    private Maintainer _assignee = Maintainer.Unassigned;
    private TriageUser _reporter = TriageUser.Unassigned;
    private IssueState _issueState = new BackLogState();
    private readonly ICollection<IssueLink> _relatedTo = new HashSet<IssueLink>();
    // ReSharper disable once CollectionNeverUpdated.Local
    private readonly ICollection<IssueLink> _relatedFrom = new HashSet<IssueLink>();

    public Issue(string project, int issueNumber, string title, string description)
    {
        Title = title;
        Description = description;
        Project = project.ToUpperInvariant();
        IssueNumber = issueNumber;
        Id = new IssueIdentifier(Project, IssueNumber);
    }

    public Issue(string project, int issueNumber, string title, string description, Priority priority, IssueType type)
        : this(project, issueNumber, title, description)
    {
        Priority = priority;
        _type = type;
    }

    private Issue()
    {
        
    }

    public IssueIdentifier Id { get; private set; } = IssueIdentifier.Empty;

    public string Project
    {
        get => _project;
        init
        {
            if (value is not { Length: > 0 } and { Length: <= 3 })
            {
                throw new ArgumentException("project must cannot be empty or greater than 3 characters", nameof(value));
            }
            _project = value;
        }
    }

    public int IssueNumber
    {
        get => _issueNumber;
        init
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "issue number cannot be negative");
            }
            _issueNumber = value;
        }
    }

    public string Title
    {
        get => _title;
        set
        {
            if (_title == value)
            {
                return;
            }

            if (value is not {Length: > 0} and {Length: <= 200})
            {
                throw new ArgumentException("title cannot be empty", nameof(value));
            }
            _title = value;
        }
    } 

    public string Description
    {
        get => _description;
        set
        {
            if (_description is not {Length: <= 500})
            {
                throw new ArgumentException("Description cannot be longer than 500", nameof(value));
            }
            _description = value ?? string.Empty;
        }
    }

    public Priority Priority { get; set; } = Priority.Low;

    public IssueType Type
    {
        get => _type;
        set
        {
            if (value is IssueType.Epic)
            {
                EpicId = null;
            }
            _type = value;
        }
    }

    /// <summary>
    /// Issue State
    /// </summary>
    public IssueState State { get; private set; } = new BackLogState();

    public TriageUser Reporter
    {
        get => _reporter;
        set
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));
            _reporter = value;
        }
    }

    public Maintainer Assignee
    {
        get => _assignee;
        set
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));
            _assignee = value;
        }
    }

    public IssueIdentifier? EpicId
    {
        get => _epicId;
        set
        {
            if (Type is IssueType.Epic)
            {
                throw new ArgumentException("Cannot assign epic to an epic", nameof(value));
            }
            _epicId = value;
        }
    }

    /// <summary>
    /// Move to next state in the sequence
    /// </summary>
    /// <param name="success">used by some states to control what the next state is</param>
    /// <returns>new state</returns>
    public void MoveStateToNext(bool success)
    {
        _issueState = _issueState.MoveToNext(success);
    }

    /// <summary>
    /// Move to the backlog
    /// </summary>
    public void MoveToBacklog()
    {
        _issueState = _issueState.MoveToBacklog();
    }

    /// <summary>
    /// Attempt to close the issue,
    /// </summary>
    /// <param name="reason">reason for the closure</param>
    /// <returns>
    /// <see langword="true"/> on success
    /// </returns>
    public bool TryClose(ClosureReason reason)
    {
        IssueState newState = _issueState.TryClose(reason);
        if (newState == _issueState)
        {
            return false;
        }

        _issueState = newState;
        return true;
    }

    /// <summary>
    /// Attempt to re-open a closed issue
    /// </summary>
    /// <returns>
    /// <see langword="true"/> on succcess
    /// </returns>
    public bool TryOpen()
    {
        IssueState newState = _issueState.TryOpen();
        if (newState == _issueState)
        {
            return false;
        }

        _issueState = newState;
        return true;
    }

    public void AddRelatedTo(LinkType link, Issue issue)
    {
        ArgumentNullException.ThrowIfNull(issue, nameof(issue));
        IssueLink issueLink = new(link, Id, this, issue.Id, issue);
        _relatedTo.Add(issueLink);
    }

    public IEnumerable<Issue> RelatedIssues => _relatedTo.Select(i => i.Right)
        .Union(_relatedFrom.Select(i => i.Left));

    public string ConcurrencyToken { get; private set; } = Guid.NewGuid().ToString();

    /// <inheritdoc />
    public override bool Equals(Entity? x, Entity? y)
    {
        return (x is null && y is null) || (x is Issue issueX && y is Issue issueY && issueX.Id == issueY.Id);
    }

    /// <inheritdoc />
    public override int GetHashCode(Entity obj)
    {
        return obj is Issue issue
            ? issue.Id.GetHashCode()
            : 0;
    }
}
