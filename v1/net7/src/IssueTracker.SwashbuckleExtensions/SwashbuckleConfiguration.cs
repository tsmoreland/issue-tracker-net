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

using System.Reflection;
using IssueTracker.ServiceDiscovery;
using IssueTracker.Shared;
using IssueTracker.SwashbuckleExtensions.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace IssueTracker.SwashBuckleExtensions;

public class SwashbuckleConfiguration : ConfigureNamedOptions<SwaggerGenOptions, IApiVersionDescriptionProvider>
{
    /// <inheritdoc />
    public SwashbuckleConfiguration(IApiVersionDescriptionProvider apiVersionDescriptionProvider, IServiceProvider serviceProvider)
        : this(Options.DefaultName, apiVersionDescriptionProvider, serviceProvider)
    {
    }

    /// <inheritdoc />
    public SwashbuckleConfiguration(string name, IApiVersionDescriptionProvider apiVersionDescriptionProvider, IServiceProvider serviceProvider)
        : base(name, apiVersionDescriptionProvider, (options, provider) => SetupAction(options, provider, serviceProvider))
    {
    }

    private static void SetupAction(SwaggerGenOptions options, IApiVersionDescriptionProvider apiVersionDescriptionProvider, IServiceProvider provider)
    {
        _ = provider; // unused for now

        options.ResolveConflictingActions(ResolveConflicts);
        options.EnableAnnotations();

        AddSwaggerDocsPerVersion(options, apiVersionDescriptionProvider.ApiVersionDescriptions);
        options.DocInclusionPredicate(
            (documentName, apiDescription) =>
            {
                if (!apiDescription.TryGetMethodInfo(out MethodInfo methodInfo))
                {
                    return false;
                }

                IEnumerable<ApiVersion> versions;
                if (apiDescription.ActionDescriptor is ControllerActionDescriptor actionDescriptor)
                {
                    versions = actionDescriptor
                        .ControllerTypeInfo
                        .AsType()
                        .GetCustomAttribute<ApiVersionAttribute>()?.Versions ?? Array.Empty<ApiVersion>();

                }
                else
                {
                    versions = methodInfo.DeclaringType?.GetCustomAttribute<ApiVersionAttribute>()?.Versions ??
                               Array.Empty<ApiVersion>();
                }

                return versions.Any(version => $"v{version}" == documentName);
            });

        IEnumerable<string> xmlFilenames = ControllerDocumentationDiscovery
            .DiscoverXmlDocumentationFiles();
        foreach (string filename in xmlFilenames)
        {
            options.IncludeXmlComments(filename);
        }

        Assembly? entryAssembly = Assembly.GetEntryAssembly();
        if (entryAssembly is not null)
        {
            string appXmlFilename = Path.ChangeExtension(entryAssembly.Location, "xml");
            if (File.Exists(appXmlFilename))
            {
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, appXmlFilename));
            }
        }

        options.OperationFilter<AddResponseHeaderOperationFilter>();
        options.OperationFilter<LinksOperationFilter>();
        options.OperationFilter<ProducesHateoasResponseTypesOperationFilter>();
        options.OperationFilter<ProducesProblemDetailsReponseOperationFilter>();
        options.CustomSchemaIds(SetSchemaName);

        static void AddSwaggerDocsPerVersion(SwaggerGenOptions options, IEnumerable<ApiVersionDescription> versionDescriptions)
        {
            foreach (ApiVersionDescription versionDescription in versionDescriptions)
            {
                (string groupName, string version) = (versionDescription.GroupName, versionDescription.ApiVersion.ToString());
                options.SwaggerDoc(groupName, BuildInfo(version));
            }
        }
        static OpenApiInfo BuildInfo(string version) =>
            new() { Title = $"Issue REST API v{version}", Version = version };
    }

    private static string SetSchemaName(Type type)
    {
        SwaggerSchemaNameAttribute? attribute = type.GetCustomAttribute<SwaggerSchemaNameAttribute>();
        return attribute is not null
            ? attribute.Name
            : type.Name;
    }

    private static ApiDescription ResolveConflicts(IEnumerable<ApiDescription> descriptions)
    {
        return descriptions.Last()!;
    }
}
