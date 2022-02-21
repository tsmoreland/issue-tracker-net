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
using IssueTracker.App.Data;
using IssueTracker.App.Model;
using IssueTracker.App.Model.Projections;
using IssueTracker.App.Model.Request;
using IssueTracker.App.Model.Response;
using Microsoft.AspNetCore.Mvc;

namespace IssueTracker.App.Controllers;

/// <summary>
/// Issue Controller
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class IssuesController : ControllerBase
{
    private readonly IssueRepository _repository;

    /// <summary>
    /// Instantiates a new instance of <see cref="IssuesController"/>
    /// </summary>
    public IssuesController(IssueRepository repository)
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
    [HttpGet]
    public IActionResult GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        if (!ValidatePaging(pageNumber, pageSize))
        {
            return BadRequest(ModelState);
        }
        else
        {
            return Ok(GetIssueSummaries(_repository, pageNumber, pageSize, cancellationToken));
        }

        static async IAsyncEnumerable<IssueSummaryDto> GetIssueSummaries(
            IssueRepository repository,
            int pageNumber, int pageSize,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            ConfiguredCancelableAsyncEnumerable<IssueSummaryProjection> issues = repository
                .GetIssueSummaries(pageNumber, pageSize, cancellationToken)
                .WithCancellation(cancellationToken);

            await foreach ((Guid id, string name) in issues)
            {
                yield return new IssueSummaryDto(id, name);
            }
        }
    }

    /// <summary>
    /// Returns issue matching <paramref name="id"/> if found
    /// </summary>
    /// <param name="id" example="1385056E-8AFA-4E09-96DF-AE12EFDF1A29">unique id of issue</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns><see cref="IssueDto"/> matching <paramref name="id"/> if found</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        Issue? issue = await _repository.GetUntrackedIssueById(id, cancellationToken);

        return issue is not null
            ? Ok(IssueDto.FromIssue(issue))
            : NotFound();
    }

    /// <summary>
    /// Adds a new issue 
    /// </summary>
    /// <param name="model">the issue to add</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] AddOrUpdateIssueDto model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ValidationProblemDetails(ModelState));
        }

        Issue issue = await _repository.AddIssue(model.ToIssue(), cancellationToken);

        return new ObjectResult(IssueDto.FromIssue(issue)) { StatusCode = StatusCodes.Status201Created };
    }

    /// <summary>
    /// Updates existing issue given by <paramref name="id"/>
    /// </summary>
    /// <param name="id">unique id of the issue to update</param>
    /// <param name="model">new values for the issue</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] AddOrUpdateIssueDto model, CancellationToken cancellationToken)
    {
        Issue? issue = await _repository.GetIssueById(id, cancellationToken);
        if (issue is null)
        {
            return NotFound();
        }

        issue.SetTitle(model.Title);
        issue.SetDescription(model.Description);
        issue.ChangePriority(model.Priority);

        await _repository.CommitAsync(cancellationToken);
        return Ok(IssueDto.FromIssue(issue));
    }

    /// <summary>
    /// Deletes the issue given by <paramref name="id"/> 
    /// </summary>
    /// <param name="id">unique id of the issue to delete</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _repository.DeleteIssueById(id, cancellationToken);
        await _repository.CommitAsync(cancellationToken);
        return new StatusCodeResult(StatusCodes.Status204NoContent);
    }

    private bool ValidatePaging(int pageNumber, int pageSize)
    {
        if (pageNumber < 1)
        {
            ModelState.AddModelError("pageNumber", "must be greater than or equal to 1");
            return false;
        }
        else if (pageSize < 1)
        {
            ModelState.AddModelError("pageSize", "must be greater than or equal to 1");
            return false;
        }
        else
        {
            return true;
        }
    }
}
