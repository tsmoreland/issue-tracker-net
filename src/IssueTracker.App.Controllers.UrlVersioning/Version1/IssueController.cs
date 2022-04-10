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
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using IssueTracker.App.Controllers.UrlVersioning.Version1.Request;
using IssueTracker.App.Controllers.UrlVersioning.Version1.Response;
using IssueTracker.Core.Requests;
using MediatR;
using Microsoft.Web.Http;
using Swashbuckle.Swagger.Annotations;
using static IssueTracker.App.Shared.Validation.PagingValidation;

namespace IssueTracker.App.Controllers.UrlVersioning.Version1;

/// <summary>
/// Issue Controller
/// </summary>
[System.Web.Http.RoutePrefix("api/v{version:apiVersion}/issues")]
[ApiVersion("1")]
public sealed class IssueController : ApiController
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Instantiates a new instance of the <see cref="IssueController"/> class.
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public IssueController()
    {
        _mediator = DependencyResolver.Current.GetService<IMediator>() ??
                    throw new InvalidOperationException("invalid services? choose a better exception");
    }

    /// <summary>
    /// Returns all issues 
    /// </summary>
    /// <param name="pageNumber" example="1" >current page number to return</param>
    /// <param name="pageSize" example="10">maximum number of items to return</param>
    /// <param name="cancellationToken">a cancellation token.</param>
    /// <returns>all issues</returns>
    [System.Web.Http.HttpGet]
    [System.Web.Http.Route("")]
    public async Task<HttpResponseMessage> GetAll(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        if (!ValidatePaging(ModelState, pageNumber, pageSize))
        {
            return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, ModelState);
        }

        List<IssueSummaryDto> summary = await IssueSummaryDto
            .MapFrom(await _mediator.Send(new GetAllIssuesRequest(pageNumber, pageSize), cancellationToken), cancellationToken)
            .ToListAsync(cancellationToken);

        return Request.CreateResponse(System.Net.HttpStatusCode.OK, summary);
    }

    /// <summary>
    /// Returns issue matching <paramref name="id"/> if found
    /// </summary>
    /// <param name="id" example="1385056E-8AFA-4E09-96DF-AE12EFDF1A29">unique id of issue</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns><see cref="IssueDto"/> matching <paramref name="id"/> if found</returns>
    [System.Web.Http.HttpGet]
    [System.Web.Http.Route("{id}")]
    [SwaggerResponse(200, "Successful Response", Type = typeof(IssueDto))]
    [SwaggerResponse(400, Description = "Invalid argument", Type = typeof(IssueSummaryDto))]
    [SwaggerResponse(404, Description = "issue not found", Type = typeof(IssueSummaryDto))]
    public async Task<HttpResponseMessage> Get(Guid id, CancellationToken cancellationToken)
    {
        IssueDto issue = IssueDto.From(await _mediator.Send(new FindIssueByIdRequest(id), cancellationToken));
        return issue is not null
            ? Request.CreateResponse(System.Net.HttpStatusCode.OK, issue)
            : Request.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, "issue not found");
    }

    /// <summary>
    /// Adds a new issue 
    /// </summary>
    /// <param name="model">the issue to add</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>newly created <see cref="IssueDto"/></returns>
    [System.Web.Http.HttpPost]
    [System.Web.Http.Route("")]
    [SwaggerResponse(201, Description = "Successful Response", Type = typeof(IssueDto))]  
    public async Task<HttpResponseMessage> Post([FromBody] AddIssueDto model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, ModelState);
        }

        IssueDto issue = IssueDto.From(await _mediator.Send(new CreateIssueRequest(model.ToModel()), cancellationToken));
        return Request.CreateResponse(System.Net.HttpStatusCode.Created, issue);
    }

    /// <summary>
    /// Updates existing issue given by <paramref name="id"/>
    /// </summary>
    /// <param name="id">unique id of the issue to update</param>
    /// <param name="model">new values for the issue</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>updated <see cref="IssueDto"/> matching <paramref name="id"/> if found</returns>
    [System.Web.Http.HttpPut]
    [System.Web.Http.Route("{id}")]
    [SwaggerResponse(200, Description = "Successful Response", Type = typeof(IssueDto))]
    [SwaggerResponse(400, "Invalid argument", Type = typeof(IssueSummaryDto))]
    [SwaggerResponse(404, "issue not found", Type = typeof(IssueSummaryDto))]
    public async Task<HttpResponseMessage> Put(Guid id, [FromBody] EditIssueDto model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, ModelState);
        }

        IssueDto issue = IssueDto.From(await _mediator.Send(new EditIssueRequest(id, model.ToModel()), cancellationToken));
        return issue is not null
            ? Request.CreateResponse(System.Net.HttpStatusCode.OK, issue)
            : Request.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, "issue not found");
    }

    /// <summary>
    /// Deletes the issue given by <paramref name="id"/> 
    /// </summary>
    /// <param name="id">unique id of the issue to delete</param>
    /// <param name="cancellationToken">A cancellation token</param>
    [System.Web.Http.HttpDelete]
    [System.Web.Http.Route("{id}")]
    [SwaggerResponse(204, Description = "Successful Response")]
    [SwaggerResponse(400, Description = "Invalid argument", Type = typeof(IssueSummaryDto))]
    [SwaggerResponse(404, Description = "issue not found", Type = typeof(IssueSummaryDto))]
    public async Task<HttpResponseMessage> Delete(Guid id, CancellationToken cancellationToken)
    {
        return await _mediator.Send(new DeleteIssueRequest(id), cancellationToken)
            ? Request.CreateResponse(System.Net.HttpStatusCode.NoContent)
            : Request.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, "issue not found");
    }
}
