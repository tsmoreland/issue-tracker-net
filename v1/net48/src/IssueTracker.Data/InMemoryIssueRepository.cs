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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using IssueTracker.Core.Model;
using IssueTracker.Core.Projections;
using IssueTracker.Data.Abstractions;

namespace IssueTracker.Data;

public sealed class InMemoryIssueRepository : IIssueRepository
{
    private readonly object _lock = new ();
    // concurrent dictionary would be better but this is faster
    private readonly Dictionary<Guid, Issue> _issuesById = new ();
    // concurrent dictionary would be more difficult for these as it's the same data indexed by different means
    private readonly Dictionary<Guid, LinkedIssue> _linkedIssuesById = new ();
    private readonly Dictionary<Guid, LinkedIssue> _linkedIssuesByChildId = new ();
    private readonly Dictionary<Guid, LinkedIssue> _linkedIssuesByParentId = new ();

    /// <inheritdoc />
    public async IAsyncEnumerable<IssueSummaryProjection> GetIssueSummaries(int pageNumber, int pageSize, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        List<IssueSummaryProjection> issues;
        lock (_lock)
        {
            issues = _issuesById.Values.Select(i => new IssueSummaryProjection(i.Id, i.Title, i.Priority, i.Type)).ToList();
        }

        foreach (IssueSummaryProjection issue in issues)
        {
            yield return issue;
        }
    }

    /// <inheritdoc />
    public IAsyncEnumerable<IssueSummaryProjection> GetParentIssues(Guid id, int pageNumber, int pageSize, CancellationToken cancellationToken) 
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public IAsyncEnumerable<IssueSummaryProjection> GetChildIssues(Guid id, int pageNumber, int pageSize, CancellationToken cancellationToken) 
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<Issue> GetUntrackedIssueById(Guid id, CancellationToken cancellationToken)
    {
        return GetIssueById(id, cancellationToken);
    }

    /// <inheritdoc />
    public Task<Issue> GetIssueById(Guid id, CancellationToken cancellationToken) 
    {
        lock (_lock)
        {
            return _issuesById.ContainsKey(id)
                ? Task.FromResult(_issuesById[id])
                : Task.FromResult<Issue>(null);
        }
    }

    /// <inheritdoc />
    public Task<Issue> AddIssue(Issue issue, CancellationToken cancellationToken) 
    {
        if (issue is null)
        {
            throw new ArgumentNullException(nameof(issue));
        }

        lock (_lock)
        {
            _issuesById[issue.Id] = issue;
        }

        return Task.FromResult(issue);
    }

    /// <inheritdoc />
    public Task<Issue> UpdateIssue(Guid id, Action<Issue> visitor, CancellationToken cancellationToken)
    {
        lock (_lock)
        {
            if (!_issuesById.ContainsKey(id))
            {
                return Task.FromResult<Issue>(null);
            }

            Issue issue = _issuesById[id];
            visitor(issue);
            return Task.FromResult(issue);
        }
    }

    /// <inheritdoc />
    public Task CommitAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task<bool> DeleteIssueById(Guid id, CancellationToken cancellationToken) 
    {
        lock (_lock)
        {
            if (_issuesById.ContainsKey(id))
            {
                _issuesById.Remove(id);
                return Task.FromResult(true);
            }
        }
        return Task.FromResult(false);
    }

    /// <inheritdoc />
    public Task<bool> IssueExists(Guid id, CancellationToken cancellationToken)
    {
        lock (_lock)
        {
            return Task.FromResult(_issuesById.ContainsKey(id));
        }
    }
}
