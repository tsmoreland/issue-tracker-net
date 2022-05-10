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

public sealed class Issue : IEntity
{
    private Guid _id = Guid.NewGuid();
    private string _title = string.Empty;
    private string _description = string.Empty;
    private Guid? _epidId;
    private string _project = string.Empty;
    private int _issueNumber = 0;
    private IssueType _type = IssueType.Defect;
    private Priority _priority = Priority.Low;
    private Maintainer _assignee = Maintainer.Unassigned;
    private TriageUser _reporter = TriageUser.Unassigned;

    public Issue(string project, string title, string description)
    {
        Title = title;
        Description = description;
        DisplayId = new IssueIdentifier(project, 0);
    }

    private Issue()
    {
        
    }

    Guid IEntity.Id => _id;
    public IssueIdentifier DisplayId { get; private set; } = IssueIdentifier.Empty;

    public string Title
    {
        get => _title;
        set
        {
            if (_title == value)
            {
                return;
            }

            if (value is not { Length: > 0 })
            {
                throw new ArgumentException("title cannot be empty", nameof(value));
            }
            _title = value;
        }
    } 

    public string Description
    {
        get => _description;
        set => _description = value ?? string.Empty;
    }

    public Priority Priority
    {
        get => _priority;
        set => _priority = value;
    }

    public IssueType Type
    {
        get => _type;
        set
        {
            if (value is IssueType.Epic)
            {
                EpidId = null;
            }
            _type = value;
        }
    }

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

    public Guid? EpidId
    {
        get => _epidId;
        set
        {
            if (Type is IssueType.Epic)
            {
                throw new ArgumentException("Cannot assign epic to an epic", nameof(value));
            }
            _epidId = value;
        }
    }

    public string ConcurrencyToken { get; private set; } = Guid.NewGuid().ToString();
    public DateTimeOffset LastModifiedTime { get; private set; } = DateTimeOffset.UtcNow;
    void IEntity.UpdateLastModifiedTime()
    {
        LastModifiedTime = DateTimeOffset.UtcNow;
    }
}
