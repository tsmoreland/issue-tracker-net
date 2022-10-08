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

namespace IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;

public sealed class Project : Entity, IEquatable<Project>
{
    private string _name = string.Empty;

    /// <summary>
    /// Instantiates a new instance of the <see cref="Project"/> class.
    /// </summary>
    /// <param name="id">porject identifier</param>
    /// <exception cref="ArgumentException">
    /// if project is not between 0 and 3 characters
    /// </exception>
    public Project(string id)
    {
        if (!IsProjectIdValid(id))
        {
            throw new ArgumentException("id is not valid, must be between 1 and 3 characters", nameof(id));
        }
        Id = id;
    }

    /// <summary>
    /// Instantiates a new instance of the <see cref="Project"/> class.
    /// </summary>
    /// <param name="id">porject identifier</param>
    /// <param name="name">project name</param>
    /// <exception cref="ArgumentException">
    /// if project is not between 0 and 3 characters
    /// if name is empty
    /// </exception>
    public Project(string id, string name)
        : this(id)
    {
        Name = name;
    }

    /// <summary>
    /// Instantiates a new instance of the <see cref="Project"/> class.
    /// </summary>
    private Project()
    {
        // provided for entity framework
    }

    public IIssueBuilder CreateIssue() => new IssueBuilder(this);

    /// <summary>
    /// Project Id
    /// </summary>
    public string Id { get; init; } = string.Empty;

    /// <summary>
    /// Project Name
    /// </summary>
    public string Name
    {
        get => _name;
        set
        {
            if (_name is not { Length: > 0 })
            {
                throw new ArgumentException("name cannot be empty", nameof(value));
            }

            _name = value;
        }
    }

    public void Deconstruct(out string id, out string name)
    {
        id = Id;
        name = Name;
    }

    /// <inheritdoc />
    public bool Equals(Project? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Id == other.Id && Name == other.Name;
    }

    /// <inheritdoc />
    public override bool Equals(Entity? x, Entity? y)
    {
        return x switch
        {
            null when y is null => true,
            Project projectX => projectX.Equals(y),
            _ => y switch
            {
                null => false,
                Project projectY => projectY.Equals(x),
                _ => y.Equals(x)
            }
        };
    }


    /// <inheritdoc />
    public override bool Equals(object? obj) =>
        ReferenceEquals(this, obj) || obj is Project other && Equals(other);

    private sealed class IssueBuilder : IIssueBuilder
    {
        private readonly Project _project;
        private int? _issueNumber;
        private string? _title;
        private string? _description;
        private Priority? _priority = null;
        private IssueType? _type;
        private IEnumerable<IssueLink>? _relatedTo;
        private IEnumerable<IssueLink>? _relatedFrom;
        private TriageUser? _reporter;
        private Maintainer? _assignee;
        private IssueIdentifier? _epicId;

        public IssueBuilder(Project project)
        {
            ArgumentNullException.ThrowIfNull(project);
            Reset();
            _project = project;
        }

        public void Reset()
        {
            _issueNumber = null;
            _title = null;
            _priority = null;
            _type = null;
            _reporter = null;
            _assignee = null;
            _epicId = null;
            _relatedFrom = null;
            _relatedTo = null;
        }

        public IIssueBuilder WithIssueNumber(int issueNumber)
        {
            _issueNumber = issueNumber;
            return this;
        }
        public IIssueBuilder WithTitle(string title)
        {
            _title = title;
            return this;
        }
        public IIssueBuilder WithDescription(string? description)
        {
            _description = description;
            return this;
        }
        public IIssueBuilder WithPriority(Priority priority)
        {
            _priority = priority;
            return this;
        }
        public IIssueBuilder WithType(IssueType type)
        {
            _type = type;
            return this;
        }
        public IIssueBuilder WithRelatedFrom(IEnumerable<IssueLink> parentIssues)
        {
            _relatedFrom = parentIssues.ToHashSet();
            return this;
        }
        public IIssueBuilder WithChildIssues(IEnumerable<IssueLink> childIssues)
        {
            _relatedTo = childIssues.ToHashSet();
            return this;
        }
        public IIssueBuilder WithReporter(TriageUser reporter)
        {
            _reporter = reporter;
            return this;
        }
        public IIssueBuilder WithAssignee(Maintainer assignee)
        {
            _assignee = assignee;
            return this;
        }

        public IIssueBuilder WithEpic(IssueIdentifier? epic)
        {
            _epicId = epic;
            return this;
        }

        public Issue Build()
        {
            Issue issue;

            if (_title is not { Length: > 0 })
            {
                throw new InvalidOperationException("title cannot be empty");
            }

            if (_description is not { Length: > 0 })
            {
                throw new InvalidOperationException("description cannot be empty");
            }

            if (_issueNumber > 0)
            {
                issue = new Issue(_project, _issueNumber.Value, _title, _description);
            }
            else
            {
                throw new InvalidOperationException("Either id or project and issue number must be set");
            }

            if (_priority is not null)
            {
                issue.Priority = _priority.Value;
            }

            if (_type is not null)
            {
                issue.Type = _type.Value;
            }

            if (_relatedFrom is not null)
            {
                foreach (IssueLink related in _relatedFrom)
                {
                    related.Left.AddRelatedTo(related.Link, issue);
                }
            }

            if (_relatedTo is not null)
            {
                foreach (IssueLink related in _relatedTo)
                {
                    issue.AddRelatedTo(related.Link, related.Left);
                }
            }

            if (_reporter is not null)
            {
                issue.Reporter = _reporter;
            }

            if (_assignee is not null)
            {
                issue.Assignee = _assignee;
            }

            if (_epicId is not null)
            {
                issue.EpicId = _epicId;
            }

            return issue;
        }
    }

    /// <summary>
    /// Validates that the project id <paramref name="id"/> meets the required formatting
    /// </summary>
    /// <param name="id">the id to verify</param>
    /// <returns><see langword="true"/> if the id is value</returns>
    public static bool IsProjectIdValid(string id)
    {
        return (id is { Length: > 0 and < 3 } && char.IsLetter(id[0]));
    }


    /// <inheritdoc />
    public override int GetHashCode() =>
        HashCode.Combine(Id, Name);

    /// <inheritdoc />
    public override int GetHashCode(Entity obj) => 
        obj is Project project 
            ? project.GetHashCode()
            : 0;

    public interface IIssueBuilder
    {
        void Reset();
        IIssueBuilder WithIssueNumber(int issueNumber);
        IIssueBuilder WithTitle(string title);
        IIssueBuilder WithDescription(string? description);
        IIssueBuilder WithPriority(Priority priority);
        IIssueBuilder WithType(IssueType type);
        IIssueBuilder WithRelatedFrom(IEnumerable<IssueLink> parentIssues);
        IIssueBuilder WithChildIssues(IEnumerable<IssueLink> childIssues);
        IIssueBuilder WithReporter(TriageUser reporter);
        IIssueBuilder WithAssignee(Maintainer assignee);
        IIssueBuilder WithEpic(IssueIdentifier? epic);
        Issue Build();
    }

}
