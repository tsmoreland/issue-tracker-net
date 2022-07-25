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
using AutoMapper;
using IssueTracker.Issues.API.REST.Version2.Converters;
using IssueTracker.Issues.API.REST.Version2.DataTransferObjects.Request;
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.Commands;
using IssueTracker.Issues.Domain.Services.Version2.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IssueTracker.Issues.API.REST.Version2.Controllers;

/// <summary>
/// Base class for Issue and specific issue type controllers
/// </summary>
public abstract class IssuesControllerBase : ControllerBase
{

    /// <summary/>
    protected IssuesControllerBase(IMediator mediator, IMapper mapper)
    {
        Mediator = mediator;
        Mapper = mapper;
    }

    /// <summary/>
    protected IMediator Mediator { get; }

    /// <summary/>
    protected IMapper Mapper { get; }

    /// <summary>
    /// Deletes the issue given by <paramref name="id"/> 
    /// </summary>
    /// <param name="id" example="APP-1234">unique id of the issue to delete</param>
    /// <param name="cancellationToken">A cancellation token</param>
    [HttpDelete("{id}")]
    [Consumes(MediaTypeNames.Application.Json, "text/json", "application/*+json", MediaTypeNames.Application.Xml)]
    [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Successful Response", ContentTypes = new[] { MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml })]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid arguments", typeof(ProblemDetails), "application/problem+json", "application/problem+xml")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Issue not found", typeof(ProblemDetails), "application/problem+json", "application/problem+xml")]
    [Filters.ValidateIssueIdServiceFilter]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
    {
        return await Mediator.Send(new DeleteIssueCommand(IssueIdentifier.FromString(id)), cancellationToken)
            ? new StatusCodeResult(StatusCodes.Status204NoContent)
            : NotFound();
    }

    /// <summary>
    /// Execute state change
    /// </summary>
    /// <param name="id" example="APP-1234">unique id of the issue to act upon</param>
    /// <param name="changeState">the state change to perform</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>empty response on success; otherwise, problem details</returns>
    [HttpPut("{id}/state")]
    [Consumes(MediaTypeNames.Application.Json, "text/json", "application/*+json", MediaTypeNames.Application.Xml)]
    [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [SwaggerResponse(StatusCodes.Status200OK, "Successful Response", ContentTypes = new[] { MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml })]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid arguments", typeof(ProblemDetails), "application/problem+json", "application/problem+xml")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Issue not found", typeof(ProblemDetails), "application/problem+json", "application/problem+xml")]
    [SwaggerResponse(StatusCodes.Status409Conflict, "state change not possible", typeof(ProblemDetails), "application/problem+json", "application/problem+xml")]
    [Filters.ValidateIssueIdServiceFilter]
    [Filters.ValidateModelStateServiceFilter]
    public Task<IActionResult> ChangeState(string id, ChangeIssueStateDto changeState, CancellationToken cancellationToken)
    {
        if (ChangeIssueStateDtoConverter.TryConvertToCommand(
                IssueIdentifier.FromString(id),
                changeState.State,
                out StateChangeCommand? command))
        {
            return Mediator.Send(command, cancellationToken)
                .ContinueWith<IActionResult>(_ => Ok(), cancellationToken);
        }

        ModelState.AddModelError("State", "unsupported state");
        return Task.FromResult<IActionResult>(BadRequest(new ValidationProblemDetails(ModelState)));

    }
}
