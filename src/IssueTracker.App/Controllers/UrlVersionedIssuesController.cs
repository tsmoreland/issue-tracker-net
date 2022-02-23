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

using System.Net.Mime;
using IssueTracker.Services.Abstractions;
using IssueTracker.Services.Abstractions.Model.Request;
using IssueTracker.Services.Abstractions.Model.Response;
using IssueTracker.SwashbuckleExtensions.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static IssueTracker.App.Validation.PagingValidation;

namespace IssueTracker.App.Controllers;

/// <summary>
/// Issue Controller
/// </summary>
[Route("api/v{version:apiVersion}/issues")]
[ApiController]
[ApiVersion("1")]
[TrimVersionFromSwagger]
public class UrlVersionedIssuesController : ControllerBase
{
    private readonly IIssuesService _service;

    /// <summary>
    /// Instantiates a new instance of <see cref="UrlVersionedIssuesController"/>
    /// </summary>
    public UrlVersionedIssuesController(IIssuesService service)
    {
        _service = service;
    }

    /// <summary>
    /// Returns all issues 
    /// </summary>
    /// <param name="pageNumber" example="1" >current page number to return</param>
    /// <param name="pageSize" example="10">maximum number of items to return</param>
    /// <param name="cancellationToken">a cancellation token.</param>
    /// <returns>all issues</returns>
    [HttpGet]
    [Consumes(MediaTypeNames.Application.Json, "text/json", "application/*+json")]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerResponse(StatusCodes.Status200OK, "Successful Response", typeof(IAsyncEnumerable<IssueSummaryDto>), MediaTypeNames.Application.Json)]
    public IActionResult GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        return ValidatePaging(ModelState, pageNumber, pageSize)
            ? Ok(_service.GetAll(pageNumber, pageSize, cancellationToken))
            : BadRequest(ModelState);
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
        IssueDto? issue = await _service.Get(id, cancellationToken);
        return issue is not null
            ? Ok(issue)
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

        IssueDto issue = await _service.Create(model, cancellationToken);
        return new ObjectResult(issue) { StatusCode = StatusCodes.Status201Created };
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
        IssueDto? issue = await _service.Update(id, model, cancellationToken);
        return issue is not null
            ? Ok(issue)
            : NotFound();
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
        await _service.Delete(id, cancellationToken);
        return new StatusCodeResult(StatusCodes.Status204NoContent);
    }
}
