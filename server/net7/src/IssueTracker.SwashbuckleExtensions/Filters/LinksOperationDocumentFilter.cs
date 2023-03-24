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

using System.Collections.Concurrent;
using System.Reflection;
using IssueTracker.Shared.AspNetCore;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace IssueTracker.SwashbuckleExtensions.Filters;

public sealed class LinksOperationDocumentFilter : IOperationFilter, IDocumentFilter
{
    private static readonly ConcurrentDictionary<(string Document, string OperationId), (OpenApiOperation Operation, List<(string OperationId, string ResponseCode)> LinkIds)> s_operations = new();

    /// <inheritdoc />
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.OperationId is not { Length: > 0 })
        {
            return;
        }

        if (!context.ApiDescription.TryGetMethodInfo(out MethodInfo? methodInfo))
        {
            return;
        }

        IEnumerable<OpenApiLinkAttribute> attributes = methodInfo.GetCustomAttributes<OpenApiLinkAttribute>().ToList();

        foreach (OpenApiLinkAttribute attribute in attributes)
        {
            string linkedOperationId = attribute.OperationId;
            string responseCode = attribute.ResponseCode.ToString();

            if (!operation.Responses.TryGetValue(responseCode, out OpenApiResponse? response))
            {
                continue;
            }

            response.Links ??= new Dictionary<string, OpenApiLink>();
            response.Links[linkedOperationId] = new OpenApiLink
            {
                OperationId = linkedOperationId,
                Description = "No Description",
                Parameters = new Dictionary<string, RuntimeExpressionAnyWrapper>() { { "id", new RuntimeExpressionAnyWrapper { Any = new OpenApiString("$request.path.id") } } }
            };
        }


        s_operations.AddOrUpdate((context.DocumentName, operation.OperationId),
            (operation, attributes.Select(a => (a.OperationId, a.ResponseCode.ToString())).ToList()), static (_, existing) => existing);
    }

    /// <inheritdoc />
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        Dictionary<string, (OpenApiOperation Operation, List<(string OperationId, string ResponseCode)> LinkIds)> operationsById = s_operations
            .ToList()
            .Where(kvp => kvp.Key.Document == context.DocumentName)
            .ToDictionary(kvp => kvp.Key.OperationId, kvp => kvp.Value);

        if (!operationsById.Any())
        {
            return;
        }

        foreach (string operationId in operationsById.Keys)
        {
            (OpenApiOperation operation, List<(string OperationId, string ResponseCode)> linkIds) = operationsById[operationId];

            foreach ((string linkedOperationId, string responseCode) in linkIds.Where(pair => operationsById.ContainsKey(pair.OperationId)))
            {
                if (!operation.Responses.TryGetValue(responseCode, out OpenApiResponse? response))
                {
                    continue;
                }

                OpenApiOperation linkedOperation = operationsById[linkedOperationId].Operation;
                response.Links ??= new Dictionary<string, OpenApiLink>();
                response.Links[linkedOperationId] = new OpenApiLink
                {
                    OperationId = linkedOperationId,
                    Description = linkedOperation.Description ?? "No Description",
                };
            }
        }

    }

}
