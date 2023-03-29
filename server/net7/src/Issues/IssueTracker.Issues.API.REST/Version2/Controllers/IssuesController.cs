//
// Copyright (c) 2022 Terry Moreland
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
using IssueTracker.Issues.API.REST.Version2.DataTransferObjects.Request;
using IssueTracker.Issues.API.REST.Version2.DataTransferObjects.ResourceParameters;
using IssueTracker.Issues.API.REST.Version2.DataTransferObjects.Response;
using IssueTracker.Issues.API.REST.VersionIndependent.DataTransferObjects.Response;
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;
using IssueTracker.Shared;
using IssueTracker.Shared.AspNetCore;
using IssueTracker.Shared.AspNetCore.ActionContraints;
using IssueTracker.Shared.AspNetCore.Extensions;
using IssueTracker.Shared.AspNetCore.Filters;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IssueTracker.Issues.API.REST.Version2.Controllers;

/// <summary>
/// Issues Controller (v2)
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/issues")]
[Tags("Issues (URL versioning)")]
[ApiVersion("2")]
public sealed class IssuesController : IssuesControllerBase
{
    private static class RouteNames
    {
        public const string Get = "GetIssueById";
        public const string GetPagedIssues = "GetIssuesInPages";
        public const string Create = "CreateIssue";
        public const string Update = "UpdateIssue";
        public const string Patch = "PatchIssue";
        public const string Delete = "DeleteIssue";
        public const string UpdateState = "UpdateIssueState";
    }

    /// <summary>
    /// Instantiates a new instance of the <see cref="IssuesController"/> class.
    /// </summary>
    public IssuesController(IMediator mediator, IMapper mapper)
        : base(mediator, mapper)
    {
    }

    /// <summary>
    /// Returns issue matching <paramref name="id"/> if found
    /// </summary>
    /// <param name="id" example="APP-1234">unique id of issue</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns><see cref="IssueDto"/> matching <paramref name="id"/> if found</returns>
    [HttpGet("{id}", Name = RouteNames.Get)]
    [HttpHead("{id}")]
    [RequestMatchesMediaType("Accept", "*/*", MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml, VendorMediaTypeNames.Application.HateoasPlusJson, VendorMediaTypeNames.Application.HateoasPlusXml)]
    [Consumes(MediaTypeNames.Application.Json,MediaTypeNames.Application.Xml)]
    [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml, VendorMediaTypeNames.Application.HateoasPlusJson, VendorMediaTypeNames.Application.HateoasPlusXml)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IssueDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [Filters.ValidateIssueIdServiceFilter]
    [ValidateModelStateServiceFilter]
    [ProducesHateoasResponseTypes(StatusCodes.Status200OK, VendorMediaTypeNames.Application.HateoasPlusJson, VendorMediaTypeNames.Application.HateoasPlusXml, typeof(IssueDto), typeof(IssueDtoWithLinks))]
    [ProducesProblemDetailsResponse]
    [OpenApiLink(RouteNames.Update, StatusCodes.Status200OK, "id, $request.path.id", Description = "update existing issue matching id")]
    [OpenApiLink(RouteNames.Patch, StatusCodes.Status200OK, "id, $request.path.id", Description = "partial update existing issue matching id")]
    [OpenApiLink(RouteNames.Delete, StatusCodes.Status200OK, "id, $request.path.id", Description = "delete existing issue matching id")]
    public Task<IActionResult> Get([FromRoute] string id, CancellationToken cancellationToken)
    {
        bool includeLinks = HttpContext.Request.Accepts(
            VendorMediaTypeNames.Application.HateoasPlusJson,
            VendorMediaTypeNames.Application.HateoasPlusXml);
        return base.GetIssueById(IssueIdentifier.FromString(id), includeLinks, cancellationToken);
    }

    /// <summary>
    /// Returns issues in pages with optional sorting, paging and filtering
    /// </summary>
    /// <param name="issuesResourceParameters"></param>
    /// <param name="cancellationToken">a cancellation token.</param>
    /// <returns>issues filtered based on the provided query parameters</returns>
    [HttpGet(Name = RouteNames.GetPagedIssues)]
    [HttpHead]
    [RequestMatchesMediaType("Accept", "*/*", MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml, VendorMediaTypeNames.Application.HateoasPlusJson, VendorMediaTypeNames.Application.HateoasPlusXml)]
    [Consumes(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml, VendorMediaTypeNames.Application.HateoasPlusJson, VendorMediaTypeNames.Application.HateoasPlusXml)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IssueSummaryPage))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesHateoasResponseTypes(StatusCodes.Status200OK, VendorMediaTypeNames.Application.HateoasPlusJson, VendorMediaTypeNames.Application.HateoasPlusXml, typeof(IssueSummaryPage), typeof(IssueSummaryPageWithLinks))]
    [ProducesProblemDetailsResponse]
    [ValidateModelStateServiceFilter]
    [OpenApiLink(RouteNames.Create, StatusCodes.Status200OK, "", Description = "create new issue matching id")]
    [OpenApiLink(RouteNames.Get, StatusCodes.Status200OK, "id, $request.path.id", Description = "get existing issue matching id")]
    [OpenApiLink(RouteNames.Update, StatusCodes.Status200OK, "id, $request.path.id", Description = "update existing issue matching id")]
    [OpenApiLink(RouteNames.Patch, StatusCodes.Status200OK, "id, $request.path.id", Description = "partial update existing issue matching id")]
    [OpenApiLink(RouteNames.Delete, StatusCodes.Status200OK, "id, $request.path.id", Description = "delete existing issue matching id")]
    public Task<IActionResult> GetPagedIssues(
        [FromQuery] IssuesResourceParameters issuesResourceParameters,
        CancellationToken cancellationToken = default)
    {
        bool includeLinks = HttpContext.Request.Accepts(
            VendorMediaTypeNames.Application.HateoasPlusJson,
            VendorMediaTypeNames.Application.HateoasPlusXml);

        return base.GetIssuesWithLinks(RouteNames.GetPagedIssues, issuesResourceParameters, includeLinks, cancellationToken);
    }

    /// <summary>
    /// Adds a new issue 
    /// </summary>
    /// <param name="model">the issue to add</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>newly created <see cref="IssueDto"/></returns>
    [HttpPost(Name = RouteNames.Create)]
    [RequestMatchesMediaType("Accept","*/*", MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml, VendorMediaTypeNames.Application.HateoasPlusJson, VendorMediaTypeNames.Application.HateoasPlusXml)]
    [Consumes(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml, VendorMediaTypeNames.Application.HateoasPlusJson, VendorMediaTypeNames.Application.HateoasPlusXml)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(IssueDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ProblemDetails))]
    [ProducesHateoasResponseTypes(StatusCodes.Status201Created, VendorMediaTypeNames.Application.HateoasPlusJson, VendorMediaTypeNames.Application.HateoasPlusXml, typeof(IssueDto), typeof(IssueDtoWithLinks))]
    [ProducesProblemDetailsResponse]
    [ValidateModelStateServiceFilter]
    [OpenApiLink(RouteNames.Get, StatusCodes.Status201Created, "id, $request.path.id", Description = "Returns issue matching id")]
    [OpenApiLink(RouteNames.Get, StatusCodes.Status201Created, "id, $request.path.id", Description = "get existing issue matching id")]
    [OpenApiLink(RouteNames.Update, StatusCodes.Status201Created, "id, $request.path.id", Description = "update existing issue matching id")]
    [OpenApiLink(RouteNames.Patch, StatusCodes.Status201Created, "id, $request.path.id", Description = "partial update existing issue matching id")]
    [OpenApiLink(RouteNames.Delete, StatusCodes.Status201Created, "id, $request.path.id", Description = "delete existing issue matching id")]
    public Task<IActionResult> CreateIssue([FromBody] AddIssueDto model, CancellationToken cancellationToken)
    {
        bool includeLinks = HttpContext.Request.Accepts(
            VendorMediaTypeNames.Application.HateoasPlusJson,
            VendorMediaTypeNames.Application.HateoasPlusXml);
        return base.Create(RouteNames.Get, model, includeLinks, cancellationToken);
    }

    /// <summary>
    /// Updates existing issue given by <paramref name="id"/>
    /// </summary>
    /// <param name="id">unique id of the issue to update</param>
    /// <param name="model">new values for the issue</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>updated <see cref="IssueDto"/> matching <paramref name="id"/> if found</returns>
    [HttpPut("{id}", Name = RouteNames.Update)]
    [RequestMatchesMediaType("Accept", "*/*", MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [Consumes(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [SwaggerResponse(StatusCodes.Status200OK, "Successful Response")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid arguments", typeof(ProblemDetails), "application/problem+json", "application/problem+xml")]
    [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, "valid data format with invalid content", typeof(ProblemDetails), VendorMediaTypeNames.ProblemDetails.Json, VendorMediaTypeNames.ProblemDetails.Xml)]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Issue not found", typeof(ProblemDetails), VendorMediaTypeNames.ProblemDetails.Json, VendorMediaTypeNames.ProblemDetails.Xml)]
    [Filters.ValidateIssueIdServiceFilter]
    [ValidateModelStateServiceFilter]
    public Task<IActionResult> Put(string id, [FromBody] EditIssueDto model, CancellationToken cancellationToken)
    {
        return base.UpdateIssue(IssueIdentifier.FromString(id), model, cancellationToken);
    }

    /// <summary>
    /// Updates existing issue given by <paramref name="id"/>
    /// </summary>
    /// <param name="id">unique id of the issue to update</param>
    /// <param name="patchDoc">patch document containing details on changes to </param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns></returns>
    [HttpPatch("{id}", Name = RouteNames.Patch)]
    [RequestMatchesMediaType("Accept", "*/*", MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [Consumes("application/json-patch+json")]
    [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [SwaggerResponse(StatusCodes.Status200OK, "Successful Response")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid arguments", typeof(ProblemDetails), VendorMediaTypeNames.ProblemDetails.Json, VendorMediaTypeNames.ProblemDetails.Xml)]
    [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, "valid data format with invalid content", typeof(ProblemDetails), "application/problem+json", "application/problem+xml")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Issue not found", typeof(ProblemDetails), "application/problem+json", "application/problem+xml")]
    [ValidateModelStateServiceFilter]
    public Task<IActionResult> Patch(string id, [FromBody] JsonPatchDocument<IssuePatch> patchDoc, CancellationToken cancellationToken)
    {
        return base.PatchIssue(IssueIdentifier.FromString(id), patchDoc, cancellationToken);
    }

    [HttpOptions]
    public IActionResult GetIssuesOptions()
    {
        Response.Headers.Add("Allow", "GET,HEAD,POST,OPTIONS");
        return Ok();
    }

    [HttpOptions("{id}")]
    [Filters.ValidateIssueIdServiceFilter]
    public IActionResult GetIssuesOptions(string id)
    {
        _ = id;
        Response.Headers.Add("Allow", "GET,HEAD,PUT,PATCH,OPTIONS");
        return Ok();
    }

    /// <summary>
    /// Deletes the issue given by <paramref name="id"/> 
    /// </summary>
    /// <param name="id" example="APP-1234">unique id of the issue to delete</param>
    /// <param name="cancellationToken">A cancellation token</param>
    [HttpDelete("{id}", Name = RouteNames.Delete)]
    [Consumes(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Successful Response", ContentTypes = new[] { MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml })]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid arguments", typeof(ProblemDetails), VendorMediaTypeNames.ProblemDetails.Json, VendorMediaTypeNames.ProblemDetails.Xml)]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Issue not found", typeof(ProblemDetails), VendorMediaTypeNames.ProblemDetails.Json, VendorMediaTypeNames.ProblemDetails.Xml)]
    [Filters.ValidateIssueIdServiceFilter]
    public Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
    {
        return base.DeleteIssue(IssueIdentifier.FromString(id), cancellationToken);
    }

    /// <summary>
    /// Execute state change
    /// </summary>
    /// <param name="id" example="APP-1234">unique id of the issue to act upon</param>
    /// <param name="changeState">the state change to perform</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>empty response on success; otherwise, problem details</returns>
    [HttpPut("{id}/state", Name = RouteNames.UpdateState)]
    [RequestMatchesMediaType("Accept", "*/*", MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [Consumes(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [SwaggerResponse(StatusCodes.Status200OK, "Successful Response", ContentTypes = new[] { MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml })]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid arguments", typeof(ProblemDetails), VendorMediaTypeNames.ProblemDetails.Json, VendorMediaTypeNames.ProblemDetails.Xml)]
    [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, "valid data format with invalid content", typeof(ProblemDetails), VendorMediaTypeNames.ProblemDetails.Json, VendorMediaTypeNames.ProblemDetails.Xml)]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Issue not found", typeof(ProblemDetails), VendorMediaTypeNames.ProblemDetails.Json, VendorMediaTypeNames.ProblemDetails.Xml)]
    [SwaggerResponse(StatusCodes.Status409Conflict, "state change not possible", typeof(ProblemDetails), VendorMediaTypeNames.ProblemDetails.Json, VendorMediaTypeNames.ProblemDetails.Xml)]
    [Filters.ValidateIssueIdServiceFilter]
    [ValidateModelStateServiceFilter]
    public Task<IActionResult> ChangeState(string id, ChangeIssueStateDto changeState, CancellationToken cancellationToken)
    {
        return base.ChangeIssueState(IssueIdentifier.FromString(id), changeState, cancellationToken);
    }

    /// <inheritdoc/>
    protected override IEnumerable<LinkDto> GetLinksForIssue(string issueId)
    {
        return GetTrimmedLinksForIssue(issueId, string.Empty);
    }

    /// <inheritdoc/>
    protected override IEnumerable<LinkDto> GetTrimmedLinksForIssue(string issueId, string ignoredLInk)
    {
        if (ignoredLInk != "self")
        {
            string rel = "self";
            if (ignoredLInk == "create-issue")
            {
                rel = "get-issue";
            }
            yield return new LinkDto(Url.Link(RouteNames.Get, new { id = issueId }), rel, "GET");
        }

        if (ignoredLInk != "create-issue")
        {
            yield return new LinkDto(Url.Link(RouteNames.Create, null), "create-issue", "POST");
        }

        if (ignoredLInk != "update-issue")
        {
            yield return new LinkDto(Url.Link(RouteNames.Update, new { id = issueId }), "update-issue", "PUT");
        }

        if (ignoredLInk != "patch-issue")
        {
            yield return new LinkDto(Url.Link(RouteNames.Patch, new { id = issueId }), "patch-issue", "PATCH");
        }

        if (ignoredLInk != "delete-issue")
        {
            yield return new LinkDto(Url.Link(RouteNames.Delete, new { id = issueId }), "delete-issue", "DELETE");
        }
    }
}
