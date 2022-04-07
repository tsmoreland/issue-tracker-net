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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using IssueTracker.App.Controllers.UrlVersioning.Version1.Response;
using IssueTracker.Core.Projections;
using IssueTracker.Core.Requests;
using MediatR;
using static IssueTracker.App.Controllers.UrlVersioning.Validation.PagingValidation;

namespace IssueTracker.App.Controllers.UrlVersioning.Version1;

[Route("api/v1/issues")]
public sealed class IssueController : ApiController
{
    private readonly IMediator _mediator;

    public IssueController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Returns all issues 
    /// </summary>
    /// <param name="pageNumber" example="1" >current page number to return</param>
    /// <param name="pageSize" example="10">maximum number of items to return</param>
    /// <param name="cancellationToken">a cancellation token.</param>
    /// <returns>all issues</returns>
    [HttpGet]
    public async Task<IHttpActionResult> GetAll(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        if (!ValidatePaging(ModelState, pageNumber, pageSize))
        {
            return BadRequest(ModelState);
        }

        List<IssueSummaryDto> summary = await IssueSummaryDto
            .MapFrom(await _mediator.Send(new GetAllIssuesRequest(pageNumber, pageSize), cancellationToken), cancellationToken)
            .ToListAsync(cancellationToken);

        return ValidatePaging(ModelState, pageNumber, pageSize)
            ? Ok(summary)
            : BadRequest(ModelState);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IHttpActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        IssueDto issue = IssueDto.From(await _mediator.Send(new FindIssueByIdRequest(id), cancellationToken));
        return issue is not null
            ? Ok(issue)
            : NotFound();
    }
}
