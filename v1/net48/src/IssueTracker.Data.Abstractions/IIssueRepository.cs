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
using System.Threading;
using System.Threading.Tasks;
using IssueTracker.Core.Model;
using IssueTracker.Core.Projections;

namespace IssueTracker.Data.Abstractions
{
    public interface IIssueRepository
    {
        /// <summary>
        /// Returns all issues 
        /// </summary>
        /// <param name="pageNumber" example="1" >current page number to return</param>
        /// <param name="pageSize" example="10">maximum number of items to return</param>
        /// <param name="cancellationToken">a cancellation token.</param>
        /// <returns>all issues</returns>
        IAsyncEnumerable<IssueSummaryProjection> GetIssueSummaries(int pageNumber, int pageSize, CancellationToken cancellationToken);

        /// <summary>
        /// Returns all parent issues of an issue given by <paramref name="id"/>
        /// </summary>
        /// <param name="id" example="1385056E-8AFA-4E09-96DF-AE12EFDF1A29">unique id of issue</param>
        /// <param name="pageNumber" example="1" >current page number to return</param>
        /// <param name="pageSize" example="10">maximum number of items to return</param>
        /// <param name="cancellationToken">a cancellation token.</param>
        /// <returns>all parent issues of an issue given by <paramref name="id"/></returns>
        IAsyncEnumerable<IssueSummaryProjection>  GetParentIssues(Guid id, int pageNumber, int pageSize, CancellationToken cancellationToken);

        /// <summary>
        /// Returns all child issues of an issue given by <paramref name="id"/>
        /// </summary>
        /// <param name="id" example="1385056E-8AFA-4E09-96DF-AE12EFDF1A29">unique id of issue</param>
        /// <param name="pageNumber" example="1" >current page number to return</param>
        /// <param name="pageSize" example="10">maximum number of items to return</param>
        /// <param name="cancellationToken">a cancellation token.</param>
        /// <returns>all child issues of an issue given by <paramref name="id"/></returns>
        IAsyncEnumerable<IssueSummaryProjection>  GetChildIssues(Guid id, int pageNumber, int pageSize, CancellationToken cancellationToken);

        Task<Issue> GetUntrackedIssueById(Guid id, CancellationToken cancellationToken);
        Task<Issue> GetIssueById(Guid id, CancellationToken cancellationToken);
        Task<Issue> AddIssue(Issue issue, CancellationToken cancellationToken);
        Task<Issue> UpdateIssue(Guid id, Action<Issue> visitor, CancellationToken cancellationToken);

        /// <summary>
        /// Persists changes to the database
        /// </summary>
        /// <param name="cancellationToken">a cancellation token.</param>
        Task CommitAsync(CancellationToken cancellationToken);

        Task<bool> DeleteIssueById(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Returns <see langword="true"/> if issue matching <paramref name="id"/> exists
        /// </summary>
        /// <param name="id">id of issue to check</param>
        /// <param name="cancellationToken">a cancellation token</param>
        /// <returns>
        /// an asynchronous task which upon completion contains <see langword="true"/>
        /// if the issue exists.
        /// </returns>
        Task<bool> IssueExists(Guid id, CancellationToken cancellationToken);
    }
}
