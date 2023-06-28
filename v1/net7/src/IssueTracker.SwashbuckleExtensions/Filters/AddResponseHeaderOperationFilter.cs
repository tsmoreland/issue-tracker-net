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

using System.Globalization;
using System.Reflection;
using IssueTracker.Shared;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace IssueTracker.SwashbuckleExtensions.Filters;

internal class AddResponseHeaderOperationFilter : IOperationFilter
{
    /// <inheritdoc />
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.ApiDescription.ActionDescriptor is not ControllerActionDescriptor actionDescriptor)
        {
            return;
        }

        IEnumerable<SwaggerResponseHeaderAttribute> methodHeaders = actionDescriptor.MethodInfo.GetCustomAttributes().OfType<SwaggerResponseHeaderAttribute>();
        List<SwaggerResponseHeaderAttribute> headers = methodHeaders.ToList();

        IEnumerable<SwaggerResponseHeaderAttribute> classHeaders = actionDescriptor.ControllerTypeInfo.GetCustomAttributes().OfType<SwaggerResponseHeaderAttribute>();
        headers.AddRange(actionDescriptor.ControllerTypeInfo.GetCustomAttributes().OfType<SwaggerResponseHeaderAttribute>());

        foreach (SwaggerResponseHeaderAttribute header in headers)
        {
            foreach (int statusCode in header.StatusCodes)
            {

                string formattedStatusCode = statusCode.ToString(CultureInfo.InvariantCulture);
                OpenApiResponse? response = operation.Responses
                    .DefaultIfEmpty(new KeyValuePair<string, OpenApiResponse?>("", null))
                    .FirstOrDefault(x => x.Key == formattedStatusCode)
                    .Value;
                if (response is null)
                {
                    continue;
                }

                response.Headers ??= new Dictionary<string, OpenApiHeader>();
                response.Headers.Add(header.Name,
                    new OpenApiHeader
                    {
                        Description = header.Description, Schema = new OpenApiSchema {Description = header.Description, Type = header.Type, Format = header.Format}
                    });
            }
        }

    }
}
