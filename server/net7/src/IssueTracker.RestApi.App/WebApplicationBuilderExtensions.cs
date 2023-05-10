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

using System.IO.Compression;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using IssueTracker.Middelware.SecurityHeaders;
using IssueTracker.RestApi.App.Filters;
using IssueTracker.ServiceDiscovery;
using IssueTracker.Shared;
using IssueTracker.Shared.AspNetCore.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.ResponseCompression;
using TSMoreland.Text.Json.NamingStrategies;
using TSMoreland.Text.Json.NamingStrategies.Strategies;

namespace IssueTracker.RestApi.App;

public static class WebApplicationBuilderExtensions
{
    public static void ConfigureServices(this WebApplicationBuilder builder, in AssemblyLocation location)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ConfigureServices(builder.Services, builder.Configuration, in location);
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration, in AssemblyLocation location)
    {
        services
            .AddProblemDetails()
            .AddHealthChecks()
            .AddNamedHealthChecks(in location);

        services
            .AddScoped<ValidateModelStateActionFilterAttribute>();

        services
            .AddControllers(options =>
            {
                options.ReturnHttpNotAcceptable = true;
                options.RespectBrowserAcceptHeader = true;
                options.ModelBinderProviders.Insert(0, new EnumModelBinderProvider());
                options.InputFormatters.Insert(0, JsonPatchInputFormatterFactory.Build());
            })
            .AddXmlSerializerFormatters()
            .ConfigureApiBehaviorOptions(apiBehaviourOptions =>
            {
                apiBehaviourOptions.SuppressConsumesConstraintForFormFileParameters = true;
                apiBehaviourOptions.SuppressInferBindingSourcesForParameters = true;
                apiBehaviourOptions.SuppressMapClientErrors = true;
                apiBehaviourOptions.SuppressModelStateInvalidFilter = true;
                apiBehaviourOptions.InvalidModelStateResponseFactory = context =>
                {
                    ProblemDetailsFactory factory = context.HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
                    ValidationProblemDetails validationProblems = factory.CreateValidationProblemDetails(context.HttpContext, context.ModelState);
                    validationProblems.Detail = "See the errors for details.";
                    validationProblems.Instance = context.HttpContext.Request.Path;
                    validationProblems.Status = StatusCodes.Status422UnprocessableEntity;
                    validationProblems.Title = "One or more validation errors occurred.";
                    // use Type to specify URL if we have one - something that would shed more light on the proble

                    return new UnprocessableEntityObjectResult(validationProblems) { ContentTypes = { "application/problem+json" } };
                };
            })
            .AddJsonOptions(jsonOptions =>
            {
                jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = JsonStrategizedNamingPolicy.SnakeCase;
                jsonOptions.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                jsonOptions.JsonSerializerOptions.Converters.Add(new JsonStrategizedStringEnumConverterFactory(new SnakeCaseEnumNamingStrategy()));
                jsonOptions.JsonSerializerOptions.TypeInfoResolver = JsonTypeInfoResolver.Combine(
                    SerializerContext.Default,
                    new DefaultJsonTypeInfoResolver());
            });

        services.AddSecurityHeaders(builder.Configuration);
        services
            .Configure<MvcOptions>(config =>
            {
                SystemTextJsonOutputFormatter? jsonOutputFormatter = config.OutputFormatters.OfType<SystemTextJsonOutputFormatter>().FirstOrDefault();
                jsonOutputFormatter?.SupportedMediaTypes?.Add(VendorMediaTypeNames.Application.HateoasPlusJson);

                XmlSerializerOutputFormatter? xmlOutputFormatter =
                    config.OutputFormatters.OfType<XmlSerializerOutputFormatter>().FirstOrDefault();
                xmlOutputFormatter?.SupportedMediaTypes?.Add(VendorMediaTypeNames.Application.HateoasPlusXml);


            });
        services
            .AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });
        services.AddApiVersioning(
            options =>
            {
                options.ReportApiVersions = true;
                options.ApiVersionReader = new AggregateApiVersionReader();
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
            });

        services
            .AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
            })
            .Configure<BrotliCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal)
            .AddHttpContextAccessor()
            .Configure<DataProtectionTokenProviderOptions>(tokenProviderOptions =>
            {
                tokenProviderOptions.TokenLifespan = TimeSpan.FromHours(1);
            });
    }
}
