//
// Copyright (c) 2023 Terry Moreland
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

using AutoMapper;
using IssueTracker.Issues.API.REST.Version2.DataTransferObjects.Response;
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;
using IssueTracker.Issues.Domain.Services.Version2.Queries;
using IssueTracker.Shared.AspNetCore.ModelBinders;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IssueTracker.Issues.API.REST.Version2.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/issuecollection")]
[Tags("Issue Collections")]
[ApiVersion("2")]
public sealed class IssueCollectionController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    /// <inheritdoc />
    public IssueCollectionController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    private static class RouteNames
    {
        public const string GetIssueCollectionByIds = nameof(GetIssueCollectionByIds);
    }

    /// <summary>
    /// Returns issues matching <paramref name="issueIds"/> or not found if any are not found
    /// </summary>
    /// <param name="issueIds">the ids of issues to locate and return; e.g. (ABC-0001, ABC-0002)</param>
    /// <param name="cancellationToken">A cancellation Token</param>
    /// <returns>issues matching <paramref name="issueIds"/></returns>
    [HttpGet("({issueIds})", Name = RouteNames.GetIssueCollectionByIds)]
    [HttpHead("({issueIds})")]
    public async Task<ActionResult<IEnumerable<IssueDto>>> GetIssueCollection(
        [ModelBinder(BinderType = typeof(ArrayModelBinder))] [FromRoute]
        IEnumerable<IssueIdentifier> issueIds, CancellationToken cancellationToken)
    {
        List<IssueIdentifier> ids = new(issueIds.Distinct());
        if (!ids.Any())
        {
            return NotFound();
        }

        GetMultipleIssuesByIdQuery query = new(ids);
        List<IssueDto> matches = await (await _mediator
            .Send(query, cancellationToken)).Select(i => _mapper.Map<IssueDto>(i))
            .ToListAsync(cancellationToken);

        return matches.Count == ids.Count
            ? Ok(matches)
            : NotFound();
    }

}
