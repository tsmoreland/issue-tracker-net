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

using System.Collections;
using System.Collections.Concurrent;
using System.Reflection;
using IssueTracker.Shared.AspNetCore;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace IssueTracker.SwashbuckleExtensions.Filters;

public sealed class LinksOperationFilter : IOperationFilter
{
    private static readonly ConcurrentDictionary<(string Document, string OperationId), (OpenApiOperation Operation, List<(string OperationId, string ResponseCode)> LinkIds)> s_operations = new();

    /// <inheritdoc />
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.OperationId is not { Length: > 0 })
        {
            return;
        }

        IEnumerable<OpenApiLinkAttribute> attributes = context.ApiDescription.ActionDescriptor.EndpointMetadata.OfType<OpenApiLinkAttribute>().ToList();
        foreach (OpenApiLinkAttribute attribute in attributes)
        {
            if (!operation.Responses.TryGetValue(attribute.ResponseCode.ToString(), out OpenApiResponse? response))
            {
                continue;
            }

            response.Links ??= new Dictionary<string, OpenApiLink>();
            if (!response.Links.ContainsKey(attribute.OperationId))
            {
                response.Links[attribute.OperationId] = BuildResponseLink(attribute);
            }
        }

        s_operations.AddOrUpdate((context.DocumentName, operation.OperationId),
            (operation, attributes.Select(a => (a.OperationId, a.ResponseCode.ToString())).ToList()), static (_, existing) => existing);
    }

    private static OpenApiLink BuildResponseLink(OpenApiLinkAttribute attribute)
    {
        OpenApiLink link = new()
        {
            OperationId = attribute.OperationId,
            Description = attribute.Description,
            Parameters = BuildParametersByName(attribute.GetParameters()),
        };
        return link;
    }

    private static Dictionary<string, RuntimeExpressionAnyWrapper> BuildParametersByName(IEnumerable<(string Name, string Expression)> parameters)
    {
        Dictionary<string, RuntimeExpressionAnyWrapper> parametersByName = new();
        foreach ((string name, string expression) in parameters)
        {
            parametersByName[name] = new RuntimeExpressionAnyWrapper() { Any = new OpenApiString(expression), };
        }

        return parametersByName;
    }
}
