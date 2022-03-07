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

using System.Runtime.CompilerServices;
using IssueTracker.Core.Model;
using IssueTracker.Core.Projections;
using IssueTracker.Data.Abstractions;
using IssueTracker.Services.Abstractions;
using IssueTracker.Services.Abstractions.Model.Request;
using IssueTracker.Services.Abstractions.Model.Response;

namespace IssueTracker.Services;

public class IssuesService : IIssuesService
{
    private readonly IIssueRepository _repository;

    public IssuesService(IIssueRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Returns all issues 
    /// </summary>
    /// <param name="pageNumber" example="1" >current page number to return</param>
    /// <param name="pageSize" example="10">maximum number of items to return</param>
    /// <param name="cancellationToken">a cancellation token.</param>
    /// <returns>all issues</returns>
    public async IAsyncEnumerable<IssueSummaryDto> GetAll(
        int pageNumber,
        int pageSize,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        ConfiguredCancelableAsyncEnumerable<IssueSummaryProjection> issues = _repository
            .GetIssueSummaries(pageNumber, pageSize, cancellationToken)
            .WithCancellation(cancellationToken);

        await foreach ((Guid id, string name) in issues)
        {
            yield return new IssueSummaryDto(id, name);
        }
    }

    /// <summary>
    /// Returns issue matching <paramref name="id"/> if found
    /// </summary>
    /// <param name="id" example="1385056E-8AFA-4E09-96DF-AE12EFDF1A29">unique id of issue</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns><see cref="IssueDto"/> matching <paramref name="id"/> if found</returns>
    public async Task<IssueDto?> Get(Guid id, CancellationToken cancellationToken)
    {
        Issue? issue = await _repository.GetUntrackedIssueById(id, cancellationToken);
        return issue is not null
            ? IssueDto.FromIssue(issue)
            : null;
    }

    /// <inheritdoc />
    public IAsyncEnumerable<IssueDto> GetParentIssues(Guid id, int pageSize, int pageNumber, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public IAsyncEnumerable<IssueDto> GetChildIssues(Guid id, int pageSize, int pageNumber, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Adds a new issue 
    /// </summary>
    /// <param name="model">the issue to add</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns></returns>
    public async Task<IssueDto> Create(AddIssueDto model, CancellationToken cancellationToken)
    {
        Issue issue = await _repository.AddIssue(model.ToIssue(), cancellationToken);
        return IssueDto.FromIssue(issue);
    }

    /// <summary>
    /// Updates existing issue given by <paramref name="id"/>
    /// </summary>
    /// <param name="id">unique id of the issue to update</param>
    /// <param name="model">new values for the issue</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns></returns>
    public async Task<IssueDto?> Update(Guid id, EditIssueDto model, CancellationToken cancellationToken)
    {
        Issue? issue = await _repository.GetIssueById(id, cancellationToken);
        if (issue is null)
        {
            return null;
        }

        issue.SetTitle(model.Title);
        issue.SetDescription(model.Description);
        issue.ChangePriority(model.Priority);

        await _repository.CommitAsync(cancellationToken);
        return IssueDto.FromIssue(issue);
    }

    /// <summary>
    /// Deletes the issue given by <paramref name="id"/> 
    /// </summary>
    /// <param name="id">unique id of the issue to delete</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns></returns>
    public async Task<bool> Delete(Guid id, CancellationToken cancellationToken)
    {
        if (!await _repository.DeleteIssueById(id, cancellationToken))
        {
            return false;
        }

        await _repository.CommitAsync(cancellationToken);
        return true;
    }
}
