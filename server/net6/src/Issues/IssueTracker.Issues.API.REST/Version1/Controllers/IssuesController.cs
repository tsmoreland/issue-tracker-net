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
//

using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using AutoMapper;
using IssueTracker.Issues.API.REST.Version1.DataTransferObjects.Request;
using IssueTracker.Issues.API.REST.Version1.DataTransferObjects.Response;
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;
using IssueTracker.Issues.Domain.ModelAggregates.Specifications;
using IssueTracker.Issues.Domain.Services.Version1.Commands;
using IssueTracker.Issues.Domain.Services.Version1.Queries;
using IssueTracker.SwashbuckleExtensions.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IssueTracker.Issues.API.REST.Version1.Controllers;

/// <summary>
/// Issue Controller
/// </summary>
[Route("api/v{version:apiVersion}/issues")]
[ApiController]
//[TrimVersionFromSwagger]
[Tags("Issues (URL Versioned)")]
[ApiVersion("1")]
public class IssuesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    /// <summary>
    /// Instantiates a new instance of <see cref="IssuesController"/>
    /// </summary>
    public IssuesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Returns all issues 
    /// </summary>
    /// <param name="pageNumber" example="1" >current page number to return</param>
    /// <param name="pageSize" example="10">maximum number of items to return</param>
    /// <param name="orderBy" example="Priority, Type, Title DESC" >order by spec</param>
    /// <param name="cancellationToken">a cancellation token.</param>
    /// <returns>all issues in batches of <paramref name="pageSize"/></returns>
    [HttpGet]
    [Consumes(MediaTypeNames.Application.Json, "text/json", "application/*+json", MediaTypeNames.Application.Xml)]
    [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [SwaggerResponse(StatusCodes.Status200OK, "Successful Response", typeof(IssueSummaryPage), MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid arguments", typeof(ProblemDetails), "application/problem+json", "application/problem+xml")]
    public async Task<IActionResult> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? orderBy = null,
        CancellationToken cancellationToken = default)
    {
        PagingOptions paging = new (pageNumber, pageSize);
        SortingOptions sorting = SortingOptions.FromString(orderBy);
        GetAllSortedAndPagedQuery query = new(paging, sorting);

        if (paging.IsValid(out string? invalidProperty, out string? errorMessage))
        {
            return Ok(IssueSummaryPage.Convert(await _mediator.Send(query, cancellationToken), _mapper));
        }

        ModelState.AddModelError(invalidProperty, errorMessage);
        return BadRequest(ModelState);
    }

    /// <summary>
    /// Returns issue matching <paramref name="id"/> if found
    /// </summary>
    /// <param name="id" example="APP-1">unique id of issue</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns><see cref="IssueDto"/> matching <paramref name="id"/> if found</returns>
    [HttpGet("{id}")]
    [Consumes(MediaTypeNames.Application.Json, "text/json", "application/*+json", MediaTypeNames.Application.Xml)]
    [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [SwaggerResponse(StatusCodes.Status200OK, "Successful Response", typeof(IssueDto), MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid arguments", typeof(ProblemDetails), "application/problem+json", "application/problem+xml")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Issue not found", typeof(ProblemDetails), "application/problem+json", "application/problem+xml")]
    [Filters.ValidateIssueIdServiceFilter]
    public async Task<IActionResult> Get(
        [RegularExpression("[A-Z][A-Z][A-Z]-[1-9][0-9]*")] string id,
        CancellationToken cancellationToken)
    {
        IssueDto? issue = _mapper.Map<IssueDto?>(await _mediator
            .Send(new FindIssueByIdQuery(IssueIdentifier.FromString(id)), cancellationToken));
        return issue is not null
            ? Ok(issue)
            : NotFound();
    }

    /// <summary>
    /// Adds a new issue 
    /// </summary>
    /// <param name="model">the issue to add</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>newly created <see cref="IssueDto"/></returns>
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json, "text/json", "application/*+json", MediaTypeNames.Application.Xml)]
    [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [SwaggerResponse(StatusCodes.Status201Created, "Successful Response", typeof(IssueDto), MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid arguments", typeof(ProblemDetails), "application/problem+json", "application/problem+xml")]
    public async Task<IActionResult> Post([FromBody] AddIssueDto model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ValidationProblemDetails(ModelState));
        }

        (string project, string title, string description, Priority priority) = model;

        IssueDto issue = _mapper.Map<IssueDto>(await _mediator
            .Send(new CreateIssueCommand(project, title, description, priority), cancellationToken));
        return new ObjectResult(issue) { StatusCode = StatusCodes.Status201Created };
    }

    /// <summary>
    /// Updates existing issue given by <paramref name="id"/>
    /// </summary>
    /// <param name="id" example="APP-1234">unique id of the issue to update</param>
    /// <param name="model">new values for the issue</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>updated <see cref="IssueDto"/> matching <paramref name="id"/> if found</returns>
    [HttpPut("{id}")]
    [Consumes(MediaTypeNames.Application.Json, "text/json", "application/*+json", MediaTypeNames.Application.Xml)]
    [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [SwaggerResponse(StatusCodes.Status200OK, "Successful Response", typeof(IssueDto), MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid arguments", typeof(ProblemDetails), "application/problem+json", "application/problem+xml")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Issue not found", typeof(ProblemDetails), "application/problem+json", "application/problem+xml")]
    [Filters.ValidateIssueIdServiceFilter]
    public async Task<IActionResult> Put(
        [RegularExpression("[A-Z][A-Z][A-Z]-[1-9][0-9]*")] string id,
        [FromBody] EditIssueDto model,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ValidationProblemDetails(ModelState));
        }
        (string? title, string? description, Priority? priority) = model;
        IssueDto? issue = _mapper.Map<IssueDto>(await _mediator.Send(
            new ModifyIssueCommand(IssueIdentifier.FromString(id), title, description, priority),
            cancellationToken));
        return issue is not null
            ? Ok(issue)
            : NotFound();
    }

    /// <summary>
    /// Deletes the issue given by <paramref name="id"/> 
    /// </summary>
    /// <param name="id" example="APP-1234">unique id of the issue to delete</param>
    /// <param name="cancellationToken">A cancellation token</param>
    [HttpDelete("{id}")]
    [Consumes(MediaTypeNames.Application.Json, "text/json", "application/*+json", MediaTypeNames.Application.Xml)]
    [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Successful Response", ContentTypes = new [] { MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml })]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid arguments", typeof(ProblemDetails), "application/problem+json", "application/problem+xml")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Issue not found", typeof(ProblemDetails), "application/problem+json", "application/problem+xml")]
    [Filters.ValidateIssueIdServiceFilter]
    public async Task<IActionResult> Delete(
        [RegularExpression("[A-Z][A-Z][A-Z]-[1-9][0-9]*")] string id,
        CancellationToken cancellationToken)
    {
        return await _mediator.Send(new DeleteIssueCommand(IssueIdentifier.FromString(id)), cancellationToken)
            ? new StatusCodeResult(StatusCodes.Status204NoContent)
            : NotFound();
    }
}