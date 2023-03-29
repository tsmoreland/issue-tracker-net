//
// Copyright © 2023 Terry Moreland
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

using IssueTracker.Shared.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace IssueTracker.SwashbuckleExtensions.Filters;

public sealed class ProducesProblemDetailsReponseOperationFilter : IOperationFilter
{
    /// <inheritdoc />
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        bool hasAttribute = context.ApiDescription.ActionDescriptor
            .EndpointMetadata.OfType<ProducesProblemDetailsResponseAttribute>().Any();
        if (!hasAttribute)
        {
            return;
        }

        OpenApiSchema schema = context.SchemaGenerator.GenerateSchema(typeof(ProblemDetails), context.SchemaRepository);
        OpenApiMediaType problemMedia = new()
        {
            Schema = schema,
        };
        foreach (string responseCode in operation.Responses.Keys)
        {
            if (!int.TryParse(responseCode, out int code) || code < 400)
            {
                continue;
            }

            operation.Responses[responseCode].Content.Clear();
            operation.Responses[responseCode].Content["application/problem+json"] = problemMedia;
            operation.Responses[responseCode].Content["application/problem+xml"] = problemMedia;
        }
    }
}
