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

using IssueTracker.Issues.Domain.DataContracts;
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.Commands;
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.State;
using Microsoft.Extensions.Logging;

namespace IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;

public sealed class Issue : Entity
{
    private string _title = string.Empty;
    private string _projectId = string.Empty;
    private string _description = string.Empty;
    private IssueIdentifier? _epicId;
    private int _issueNumber;
    private IssueType _type = IssueType.Defect;
    private readonly ICollection<IssueLink> _parents = new HashSet<IssueLink>();
    private readonly ICollection<IssueLink> _children = new HashSet<IssueLink>();
    private readonly ICollection<Comment> _comments = new HashSet<Comment>();
    private DateTimeOffset? _startTime;
    private DateTimeOffset? _stopTime;

    public Issue(Project project, int issueNumber, string title, string description)
    {
        ArgumentNullException.ThrowIfNull(project);

        Title = title;
        Description = description;
        ProjectId = project.Id.ToUpperInvariant();
        Project = project;
        IssueNumber = issueNumber;
        Id = new IssueIdentifier(ProjectId, IssueNumber);
    }

    public Issue(Project project, int issueNumber, string title, string description, Priority priority, IssueType type)
        : this(project, issueNumber, title, description)
    {
        Priority = priority;
        _type = type;
    }

    private Issue()
    {
        Project = null!;
    }

    public IssueIdentifier Id { get; private set; } = IssueIdentifier.Empty;

    public string ProjectId
    {
        get => _projectId;
        init => _projectId = Project.IsProjectIdValid(value)
            ? value
            : throw new ArgumentException("invalid project id");
    }

    public Project Project { get; init; }

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

            if (value is not { Length: > 0 } and { Length: <= 200 })
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
            if (_description is not { Length: <= 500 })
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

    public User? Reporter { get; set; }

    public User? Assignee { get; set; }

    /// <summary>
    /// Time when issues was first opened, can only be set once
    /// </summary>
    public DateTimeOffset? StartTime
    {
        get => _startTime;
        set
        {
            if (value is null)
            {
                throw new ArgumentException("Start time cannot be set to null", nameof(value));
            }
            _startTime = value;
        }
    }
    /// <summary>
    /// Time when issue was completed, or set to a variation of non-defect; can only be set once
    /// </summary>
    public DateTimeOffset? StopTime
    {
        get => _stopTime;
        set
        {
            if (value is null)
            {
                throw new ArgumentException("Stop time cannot be set to null", nameof(value));
            }

            if (value < _startTime)
            {
                throw new ArgumentException("Stop time cannot be less than start time", nameof(value));
            }
            _stopTime = value;
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
    /// Execute <paramref name="command"/> updating state if necessary
    /// </summary>
    /// <param name="command">state change command to execute</param>
    public bool Execute(StateChangeCommand command)
    {
        if (!State.CanExecute(command))
        {
            return false;
        }

        State = State.Execute(command, this);
        return true;
    }

    public void AddLinkToParent(LinkType link, Issue issue)
    {
        ArgumentNullException.ThrowIfNull(issue);
        IssueLink issueLink = new(link, issue.Id, issue, Id, this);
        _parents.Add(issueLink);
    }

    public void AddLinkToChild(LinkType link, Issue issue)
    {
        ArgumentNullException.ThrowIfNull(issue);
        IssueLink issueLink = new(link, Id, this, issue.Id, issue);
        _children.Add(issueLink);
    }

    /// <summary>
    /// Add a new comment to the collection of comments associated with this issue
    /// </summary>
    /// <param name="author">comment author</param>
    /// <param name="content">content to add</param>
    /// <returns>
    /// the Comment to be added
    /// </returns>
    public void AddCommentOrThrow(User author, string content)
    {
        ArgumentNullException.ThrowIfNull(author);
        Comment comment = new(Id, author, content);
        _comments.Add(comment);
    }

    public IEnumerable<(LinkType, Issue)> ChildLinks =>
        _parents
            .Where(e => e.ParentId == Id)
            .Select(e => (e.LinkType, e.Child))
            .ToList()
            .AsReadOnly();

    public IEnumerable<(LinkType, Issue)> Parents =>
        _parents
            .Where(e => e.ChildId == Id)
            .Select(e => (e.LinkType, e.Parent))
            .ToList()
            .AsReadOnly();

    public IEnumerable<Comment> Comments =>
        _comments.ToList().AsReadOnly();

    public void PatchOrThrow(string key, object? value)
    {
        switch (key)
        {
            case nameof(Title):
                Title = GetValueOrThrow<string>(value);
                break;
            case nameof(Description):
                Description = GetValueOrThrow<string>(value);
                break;
            case nameof(EpicId):
                EpicId = GetValueOrThrow<IssueIdentifier?>(value);
                break;
            case nameof(Type):
                Type = GetValueOrThrow<IssueType>(value);
                break;
            case nameof(Assignee):
                Assignee = GetValueOrThrow<User>(value);
                break;
            case nameof(Reporter):
                Reporter = GetValueOrThrow<User>(value);
                break;
            case nameof(StartTime):
                StartTime = GetValueOrThrow<DateTimeOffset?>(value);
                break;
            case nameof(StopTime):
                StopTime = GetValueOrThrow<DateTimeOffset?>(value);
                break;
            // TODO:
            //case nameof(RelatedTo):
            //case nameof(RelatedFrom):
            default:
                throw new ArgumentException($"Unrecognized property {key}", nameof(key));
        }

        static T GetValueOrThrow<T>(object? value)
        {
            return value is T typedValue
                ? typedValue
                : throw new ArgumentException("value does not have expected type", nameof(value));
        }
    }

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
