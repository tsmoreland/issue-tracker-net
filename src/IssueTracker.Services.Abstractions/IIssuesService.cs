﻿// 
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

using Version1Request = IssueTracker.Services.Abstractions.Model.Version1.Request;
using Version1Response = IssueTracker.Services.Abstractions.Model.Version1.Response;
using Version2Response = IssueTracker.Services.Abstractions.Model.Version2.Response;

namespace IssueTracker.Services.Abstractions;

public interface IIssuesService
{
    /// <summary>
    /// Returns all issues 
    /// </summary>
    /// <param name="pageNumber" example="1" >current page number to return</param>
    /// <param name="pageSize" example="10">maximum number of items to return</param>
    /// <param name="cancellationToken">a cancellation token.</param>
    /// <returns>all issues</returns>
    IAsyncEnumerable<Version1Response.IssueSummaryDto> GetAll(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);

    /// <summary>
    /// Returns issue matching <paramref name="id"/> if found
    /// </summary>
    /// <param name="id" example="1385056E-8AFA-4E09-96DF-AE12EFDF1A29">unique id of issue</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns><see cref="Version1Response.IssueDto"/> matching <paramref name="id"/> if found</returns>
    Task<Version1Response.IssueDto?> Get(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Returns issue matching <paramref name="id"/> if found; optionally 
    /// with related issues
    /// </summary>
    /// <param name="id" example="1385056E-8AFA-4E09-96DF-AE12EFDF1A29">unique id of issue</param>
    /// <param name="includeParents" example="false">include parent issues</param>
    /// <param name="includeChildren" example="true">include child issues</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns><see cref="Version2Response.IssueDto"/> matching <paramref name="id"/> if found</returns>
    Task<Version2Response.IssueDto> GetWithLinkedIssues(Guid id, bool includeParents, bool includeChildren, CancellationToken cancellationToken);

    /// <summary>
    /// Returns all parent issues of an issue given by <paramref name="id"/>
    /// </summary>
    /// <param name="id" example="1385056E-8AFA-4E09-96DF-AE12EFDF1A29">unique id of issue</param>
    /// <param name="pageNumber" example="1" >current page number to return</param>
    /// <param name="pageSize" example="10">maximum number of items to return</param>
    /// <param name="cancellationToken">a cancellation token.</param>
    /// <returns>all parent issues of an issue given by <paramref name="id"/></returns>
    IAsyncEnumerable<Version2Response.IssueDto>  GetParentIssues(Guid id, int pageSize, int pageNumber, CancellationToken cancellationToken);

    /// <summary>
    /// Returns all child issues of an issue given by <paramref name="id"/>
    /// </summary>
    /// <param name="id" example="1385056E-8AFA-4E09-96DF-AE12EFDF1A29">unique id of issue</param>
    /// <param name="pageNumber" example="1" >current page number to return</param>
    /// <param name="pageSize" example="10">maximum number of items to return</param>
    /// <param name="cancellationToken">a cancellation token.</param>
    /// <returns>all child issues of an issue given by <paramref name="id"/></returns>
    IAsyncEnumerable<Version2Response.IssueDto>  GetChildIssues(Guid id, int pageSize, int pageNumber, CancellationToken cancellationToken);

    /// <summary>
    /// Adds a new issue 
    /// </summary>
    /// <param name="model">the issue to add</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>the newly created issue</returns>
    Task<Version1Response.IssueDto> Create(Version1Request.AddIssueDto model, CancellationToken cancellationToken);

    /// <summary>
    /// Updates existing issue given by <paramref name="id"/>
    /// </summary>
    /// <param name="id">unique id of the issue to update</param>
    /// <param name="model">new values for the issue</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>The updated Issue</returns>
    Task<Version1Response.IssueDto?> Update(Guid id, Version1Request.EditIssueDto model, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes the issue given by <paramref name="id"/> 
    /// </summary>
    /// <param name="id">unique id of the issue to delete</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns><see langword="true"/> if issue was found and deleted</returns>
    Task<bool> Delete(Guid id, CancellationToken cancellationToken);
}
