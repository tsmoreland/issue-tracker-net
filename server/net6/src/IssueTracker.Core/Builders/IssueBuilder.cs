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

using IssueTracker.Core.Model;
using IssueTracker.Core.ValueObjects;

namespace IssueTracker.Core.Builders;

public sealed class IssueBuilder
{
    private Guid? _issueid;
    private IssueIdentifier? _id;
    private string? _project;
    private string? _title;
    private string? _description;
    private Priority _priority = Priority.Low;
    private IssueType? _type;
    private IEnumerable<LinkedIssue>? _parentIssueEntities;
    private IEnumerable<LinkedIssue>? _childIssueEntities;
    private TriageUser? _reporter;
    private Maintainer? _assignee;
    private Issue? _epic;

    public IssueBuilder()
    {
        Reset();
    }

    public void Reset()
    {
        _id = null;
        _issueid = null;
        _project = null;
        _title = null;
        _priority = Priority.Low;
        _type = null;
        _reporter = null;
        _assignee = null;
        _epic = null;
        _parentIssueEntities = null;
        _childIssueEntities = null;
    }


    public IssueBuilder WithId(Guid issueId, IssueIdentifier id)
    {
        _issueid = issueId;
        _id = id;
        return this;
    }

    public IssueBuilder WithProject(string project)
    {
        _project = project;
        return this;
    }
    public IssueBuilder WithTitle(string title)
    {
        _title = title;
        return this;
    }
    public IssueBuilder WithDescription(string? description)
    {
        _description = description;
        return this;
    }
    public IssueBuilder WithPriority(Priority priority)
    {
        _priority = priority;
        return this;
    }
    public IssueBuilder WithType(IssueType type)
    {
        _type = type;
        return this;
    }
    public IssueBuilder WithParentIssues(IEnumerable<LinkedIssue> parentIssues)
    {
        _parentIssueEntities = parentIssues.ToHashSet();
        return this;
    }
    public IssueBuilder WithChildIssues(IEnumerable<LinkedIssue> childIssues)
    {
        _childIssueEntities = childIssues.ToHashSet();
        return this;
    }
    public IssueBuilder WithReporter(TriageUser reporter)
    {
        _reporter = reporter;
        return this;
    }
    public IssueBuilder WithAssignee(Maintainer assignee)
    {
        _assignee = assignee;
        return this;
    }

    public IssueBuilder WithEpic(Issue? epic)
    {
        _epic = epic;
        return this;
    }

    public Issue Build()
    {
        Issue issue;

        if (_project is not { Length: > 0 })
        {
            throw new InvalidOperationException("project cannot be empty");
        }

        if (_title is not { Length: > 0 })
        {
            throw new InvalidOperationException("title cannot be empty");
        }

        if (_description is not { Length: > 0 })
        {
            throw new InvalidOperationException("description cannot be empty");
        }

        if (_issueid is not null && _id is not null)
        {
            issue = new Issue(_issueid.Value, _id.Value)
            {
                Project = _project,
            };
            issue.SetTitle(_title);
            issue.SetDescription(_description);
            issue.SetPriority(_priority);
        }
        else
        {
            issue = new Issue(_project, _title, _description, _priority);
        }

        if (_type is not null)
        {
            issue.SetIssueType(_type.Value);
        }

        if (_parentIssueEntities is not null)
        {
            foreach (var parent in _parentIssueEntities)
            {
                parent.ParentIssue.LinkToIssue(parent.LinkType, issue);
            }
        }

        if (_childIssueEntities is not null)
        {
            foreach (LinkedIssue child in _childIssueEntities)
            {
                issue.LinkToIssue(child.LinkType, child.ChildIssue);
            }
        }

        if (_reporter is not null)
        {
            issue.SetReporter(_reporter);
        }

        if (_assignee is not null)
        {
            issue.SetAssignee(_assignee);
        }

        if (_epic is not null)
        {
            issue.SetEpic(_epic);
        }

        return issue;
    }



}
