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

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IssueTracker.Core.Model;
using IssueTracker.Core.Projections;
using IssueTracker.Data.Abstractions;

namespace IssueTracker.Data
{
    public sealed class IssueRepository : IIssueRepository
    {
        private ConcurrentDictionary<Guid, Issue> _issuesById = new ConcurrentDictionary<Guid, Issue>();


        /// <inheritdoc />
        public IAsyncEnumerable<IssueSummaryProjection> GetIssueSummaries(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<Issue> GetIssueById(Guid id, CancellationToken cancellationToken) 
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<Issue> AddIssue(Issue issue, CancellationToken cancellationToken) 
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task CommitAsync(CancellationToken cancellationToken) 
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<bool> DeleteIssueById(Guid id, CancellationToken cancellationToken) 
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<bool> IssueExists(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
