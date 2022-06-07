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

namespace IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;

public sealed class IssueBuilder
{
    private IssueIdentifier? _id;
    private string? _project;
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

    public IssueBuilder()
    {
        Reset();
    }

    public void Reset()
    {
        _id = null;
        _project = null;
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


    public IssueBuilder WithId(IssueIdentifier id)
    {
        if (_issueNumber is not null || _project is not null)
        {
            throw new InvalidOperationException("cannot set id if issue number or project have been set");
        }

        _id = id;
        return this;
    }

    public IssueBuilder WithProject(string project)
    {
        if (_id is not null)
        {
            throw new InvalidOperationException("Cannot set project when id has been set");
        }

        _project = project;
        return this;
    }
    public IssueBuilder WithIssueNumber(int issueNumber)
    {
        if (_id is not null)
        {
            throw new InvalidOperationException("Cannot set issue number when id has been set");
        }

        _issueNumber = issueNumber;
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
    public IssueBuilder WithRelatedFrom(IEnumerable<IssueLink> parentIssues)
    {
        _relatedFrom = parentIssues.ToHashSet();
        return this;
    }
    public IssueBuilder WithChildIssues(IEnumerable<IssueLink> childIssues)
    {
        _relatedTo = childIssues.ToHashSet();
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

    public IssueBuilder WithEpic(IssueIdentifier? epic)
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

        if (_id is not null)
        {
            issue = new Issue(_id.Value.Project, _id.Value.IssueNumber, _title, _description);
        }
        else if (_project is { Length: > 0 } && _issueNumber > 0)
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
