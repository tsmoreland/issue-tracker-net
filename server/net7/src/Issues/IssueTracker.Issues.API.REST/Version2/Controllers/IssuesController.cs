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
using IssueTracker.Shared.AspNetCore.ActionContraints;
using IssueTracker.Shared.AspNetCore.Filters;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
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
    private readonly ProblemDetailsFactory _problemDetailsFactory;

    private static class RouteNames
    {
        public const string Get = "GetIssueById";
        public const string GetPagedIssues = "GetIssuesInPages";
        public const string Create = "CreateIssue";
        public const string Update = "UpdateIssue";
        public const string Patch = "PatchIssue";
        public const string Delete = "DeleteIssue";
        public const string UpdateState = "UpdateIssueState";

        public const string GetWithHateoasResponse = "GetIssueByIdWithLinks";
        public const string GetPagedIssuesWithHateoasResponse = "GetPagedIssuesWithHateoasResponse";
        public const string CreateWithHateoasResponse = "CreateWithHateoasResponse";
    }

    /// <summary>
    /// Instantiates a new instance of the <see cref="IssuesController"/> class.
    /// </summary>
    public IssuesController(IMediator mediator, IMapper mapper, ProblemDetailsFactory problemDetailsFactory)
        : base(mediator, mapper)
    {
        // move to filter
        _problemDetailsFactory = problemDetailsFactory ?? throw new ArgumentNullException(nameof(problemDetailsFactory));
    }

    /// <summary>
    /// Returns issue matching <paramref name="id"/> if found
    /// </summary>
    /// <param name="id" example="APP-1234">unique id of issue</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns><see cref="IssueDto"/> matching <paramref name="id"/> if found</returns>
    [HttpGet("{id}", Name = RouteNames.Get)]
    [HttpHead("{id}")]
    [RequestMatchesMediaType("Accept", "*/*", MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [Consumes(MediaTypeNames.Application.Json,MediaTypeNames.Application.Xml)]
    [SwaggerResponse(StatusCodes.Status200OK, "Successful Response", typeof(IssueDto), MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid arguments", typeof(ProblemDetails), "application/problem+json", "application/problem+xml")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Issue not found", typeof(ProblemDetails), "application/problem+json", "application/problem+xml")]
    [Filters.ValidateIssueIdServiceFilter]
    [ValidateModelStateServiceFilter]
    public Task<IActionResult> Get([FromRoute] string id, CancellationToken cancellationToken)
    {
        return base.GetIssueById(IssueIdentifier.FromString(id), false, cancellationToken);
    }

    /// <summary>
    /// Returns issue matching <paramref name="id"/> if found
    /// </summary>
    /// <param name="id" example="APP-1234">unique id of issue</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns><see cref="IssueDto"/> matching <paramref name="id"/> if found</returns>
    [HttpGet("{id}", Name = RouteNames.GetWithHateoasResponse)]
    [HttpHead("{id}")]
    [RequestMatchesMediaType("Accept", VendorMediaTypeNames.Application.HateoasPlusJson, VendorMediaTypeNames.Application.HateoasPlusXml)]
    [Consumes(MediaTypeNames.Application.Json,MediaTypeNames.Application.Xml)]
    [SwaggerResponse(StatusCodes.Status200OK, "Successful Response", typeof(IssueDtoWithLinks), VendorMediaTypeNames.Application.HateoasPlusJson, VendorMediaTypeNames.Application.HateoasPlusXml)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid arguments", typeof(ProblemDetails), VendorMediaTypeNames.ProblemDetails.Json, VendorMediaTypeNames.ProblemDetails.Xml)]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Issue not found", typeof(ProblemDetails), VendorMediaTypeNames.ProblemDetails.Json, VendorMediaTypeNames.ProblemDetails.Xml)]
    [Filters.ValidateIssueIdServiceFilter]
    [ValidateModelStateServiceFilter]
    public Task<IActionResult> GetWithHateoasResponse([FromRoute] string id, CancellationToken cancellationToken)
    {
        return base.GetIssueById(IssueIdentifier.FromString(id), true, cancellationToken);
    }

    /// <summary>
    /// Returns issues in paginges with optional sorting, paging and filtering
    /// </summary>
    /// <param name="issuesResourceParameters"></param>
    /// <param name="cancellationToken">a cancellation token.</param>
    /// <returns>issues filtered based on the provided query parameters</returns>
    [HttpGet(Name = RouteNames.GetPagedIssues)]
    [HttpHead]
    [RequestMatchesMediaType("Accept", "*/*", MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [Consumes(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [SwaggerResponse(StatusCodes.Status200OK, "Successful Response", typeof(IssueSummaryPage), MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid arguments", typeof(ProblemDetails), VendorMediaTypeNames.ProblemDetails.Json, VendorMediaTypeNames.ProblemDetails.Xml)]
    [ValidateModelStateServiceFilter]
    public Task<IActionResult> GetPagedIssues(
        [FromQuery] IssuesResourceParameters issuesResourceParameters,
        CancellationToken cancellationToken = default)
    {
        return base.GetIssuesWithLinks(RouteNames.GetPagedIssues, issuesResourceParameters, false, cancellationToken);
    }

    /// <summary>
    /// Returns issues in paginges with optional sorting, paging and filtering
    /// </summary>
    /// <param name="issuesResourceParameters"></param>
    /// <param name="cancellationToken">a cancellation token.</param>
    /// <returns>issues filtered based on the provided query parameters</returns>
    [HttpGet(Name = RouteNames.GetPagedIssuesWithHateoasResponse)]
    [HttpHead]
    [RequestMatchesMediaType("Accept", VendorMediaTypeNames.Application.HateoasPlusJson, VendorMediaTypeNames.Application.HateoasPlusXml)]
    [Consumes(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [SwaggerResponse(StatusCodes.Status200OK, "Successful Response", typeof(IssueSummaryPageWithLinks), VendorMediaTypeNames.Application.HateoasPlusJson, VendorMediaTypeNames.Application.HateoasPlusXml)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid arguments", typeof(ProblemDetails), VendorMediaTypeNames.ProblemDetails.Json, VendorMediaTypeNames.ProblemDetails.Xml)]
    [ValidateModelStateServiceFilter]
    public Task<IActionResult> GetPagedIssuesWithHateoasResponse(
        [FromQuery] IssuesResourceParameters issuesResourceParameters,
        CancellationToken cancellationToken = default)
    {
        return base.GetIssuesWithLinks(RouteNames.GetPagedIssues, issuesResourceParameters, false, cancellationToken);
    }

    /// <summary>
    /// Adds a new issue 
    /// </summary>
    /// <param name="model">the issue to add</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>newly created <see cref="IssueDto"/></returns>
    [HttpPost(Name = RouteNames.Create)]
    [RequestMatchesMediaType("Accept", "*/*", MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [Consumes(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [SwaggerResponse(StatusCodes.Status201Created, "Successful Response", typeof(IssueDtoWithLinks), MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid arguments", typeof(ProblemDetails), VendorMediaTypeNames.ProblemDetails.Json, VendorMediaTypeNames.ProblemDetails.Xml)]
    [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, "valid data format with invalid content", typeof(ProblemDetails), VendorMediaTypeNames.ProblemDetails.Json, VendorMediaTypeNames.ProblemDetails.Xml)]
    [ValidateModelStateServiceFilter]
    public Task<IActionResult> CreateIssue([FromBody] AddIssueDto model, CancellationToken cancellationToken)
    {
        return base.Create(RouteNames.Get, model, false, cancellationToken);
    }

    /// <summary>
    /// Adds a new issue 
    /// </summary>
    /// <param name="model">the issue to add</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>newly created <see cref="IssueDto"/></returns>
    [HttpPost(Name = RouteNames.CreateWithHateoasResponse)]
    [RequestMatchesMediaType("Accept",VendorMediaTypeNames.Application.HateoasPlusJson, VendorMediaTypeNames.Application.HateoasPlusXml)]
    [Consumes(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [SwaggerResponse(StatusCodes.Status201Created, "Successful Response", typeof(IssueDtoWithLinks), VendorMediaTypeNames.Application.HateoasPlusJson, VendorMediaTypeNames.Application.HateoasPlusXml)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid arguments", typeof(ProblemDetails), VendorMediaTypeNames.ProblemDetails.Json, VendorMediaTypeNames.ProblemDetails.Xml)]
    [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, "valid data format with invalid content", typeof(ProblemDetails), VendorMediaTypeNames.ProblemDetails.Json, VendorMediaTypeNames.ProblemDetails.Xml)]
    [ValidateModelStateServiceFilter]
    public Task<IActionResult> CreateIssueWithHateoasResponse([FromBody] AddIssueDto model, CancellationToken cancellationToken)
    {
        return base.Create(RouteNames.Get, model, false, cancellationToken);
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
        yield return new LinkDto(Url.Link(RouteNames.GetWithHateoasResponse, new { id = issueId }), "self", "GET");
        yield return new LinkDto(Url.Link(RouteNames.CreateWithHateoasResponse, null), "create-issue", "POST");
        yield return new LinkDto(Url.Link(RouteNames.Update, new { id = issueId }), "update-issue", "PUT");
        yield return new LinkDto(Url.Link(RouteNames.Patch, new { id = issueId }), "patch-issue", "PATCH");
        yield return new LinkDto(Url.Link(RouteNames.Delete, new { id = issueId }), "delete-issue", "DELETE");
    }
}
