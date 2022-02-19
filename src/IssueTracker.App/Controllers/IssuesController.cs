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
using IssueTracker.App.Model.Response;
using Microsoft.AspNetCore.Mvc;

namespace IssueTracker.App.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IssuesController : ControllerBase
{
    private readonly IssueRepository _repository;

    public IssuesController(IssueRepository repository)
    {
        _repository = repository;
    }

    // GET: api/<IssueController>
    [HttpGet]
    public async IAsyncEnumerable<IssueSummaryDto> GetAll([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        ConfiguredCancelableAsyncEnumerable<IssueSummaryProjection> issues = _repository
            .GetIssueSummaries(cancellationToken)
            .WithCancellation(cancellationToken);

        await foreach ((Guid id, string name) in issues)
        {
            yield return new IssueSummaryDto(id, name);
        }
    }

    // GET api/<IssueController>/5
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        Issue? issue = await _repository.GetIssueById(id, cancellationToken);

        return issue is not null
            ? Ok(IssueDto.FromIssue(issue))
            : NotFound();
    }

    /*
    // POST api/<IssueController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/<IssueController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<IssueController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
    */
}
