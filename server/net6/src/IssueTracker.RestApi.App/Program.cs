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

using System.IO.Compression;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using AspNetCoreRateLimit;
using Hellang.Middleware.ProblemDetails;
using IssueTracker.Issues.Domain.DataContracts;
using IssueTracker.Middelware.SecurityHeaders;
using IssueTracker.RestApi.App;
using IssueTracker.RestApi.App.Filters;
using IssueTracker.ServiceDiscovery;
using IssueTracker.Shared.Contracts;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Caching;
using Polly.Registry;
using Serilog;
using Tcell.Agent.AspNetCore;
using TSMoreland.Text.Json.NamingStrategies;
using TSMoreland.Text.Json.NamingStrategies.Strategies;

AssemblyLocation? maybeLocation = AssemblyLocation.FromAssembly(Assembly.GetEntryAssembly());
if (maybeLocation is null)
{
    return;
}
AssemblyLocation location = maybeLocation.Value;

HostingStartupDiscovery
    .DiscoverUnloadedAssembliesContainingHostingStartup(in location)
    .SetHostingStartupAssemblies();

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Host
    .UseSerilog((context, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(context.Configuration));
builder.WebHost
    .ConfigureKestrel(kestrelServerOptions => kestrelServerOptions.AddServerHeader = false)
    .UseTcellAgent(environemnt => (environemnt.IsProduction() || environemnt.IsDevelopment()) && File.Exists(Path.Combine(environemnt.ContentRootPath, "tcell_agent.config")));

// Add services to the container.

builder.Services
    .AddProblemDetails()
    .AddHealthChecks()
    .AddNamedHealthChecks(in location);

builder.Services
    .AddControllers(options =>
    {
        options.RespectBrowserAcceptHeader = true;
        options.ModelBinderProviders.Insert(0, new EnumModelBinderProvider());
        options.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
    })
    .AddXmlSerializerFormatters()
    .ConfigureApiBehaviorOptions(apiBehaviourOptions =>
    {
        apiBehaviourOptions.SuppressConsumesConstraintForFormFileParameters = true;
        apiBehaviourOptions.SuppressInferBindingSourcesForParameters = true;
        apiBehaviourOptions.SuppressMapClientErrors = true;
        apiBehaviourOptions.SuppressModelStateInvalidFilter = true;
    })
    .AddJsonOptions(jsonOptions =>
    {
        jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = JsonStrategizedNamingPolicy.SnakeCase;
        jsonOptions.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        jsonOptions.JsonSerializerOptions.Converters.Add(new JsonStrategizedStringEnumConverterFactory(new SnakeCaseEnumNamingStrategy()));
        jsonOptions.JsonSerializerOptions.AddContext<SerializerContext>();
    });

builder.Services.AddSecurityHeaders(builder.Configuration);
builder.Services
    .AddVersionedApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'V";
        options.SubstituteApiVersionInUrl = true;
    });
builder.Services.AddApiVersioning(
    options =>
    {
        options.ReportApiVersions = true;
        options.ApiVersionReader = new AggregateApiVersionReader();
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
    });

builder.Services
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

builder.Services
    .AddMemoryCache()
    .AddSingleton<ISyncCacheProvider, Polly.Caching.Memory.MemoryCacheProvider>()
    .AddSingleton<IAsyncCacheProvider, Polly.Caching.Memory.MemoryCacheProvider>()
    .AddSingleton<IReadOnlyPolicyRegistry<string>, PolicyRegistry>(serviceProvider =>
    {
        PolicyRegistry registry = new()
        {
            {
                nameof(HttpResponseMessage), Policy.CacheAsync(
                    serviceProvider.GetRequiredService<IAsyncCacheProvider>().AsyncFor<HttpResponseMessage>(),
                    TimeSpan.FromMinutes(5))
            },
            {
                "health",
                Policy.Cache(
                    serviceProvider.GetRequiredService<ISyncCacheProvider>().For<string>(),
                    TimeSpan.FromMinutes(2))
            }
        };
        return registry;
    })
    .Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"))
    .AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>()
    .AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>()
    .AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>()
    .AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

WebApplication app = builder.Build();
using (IServiceScope scope = app.Services.CreateScope())
{
    IIssueDataMigration migration = scope.ServiceProvider.GetRequiredService<IIssueDataMigration>();
    await migration.MigrateAsync();
    await migration.ResetAndRepopultateAsync();
}

app.UseSecurityHeaders();
app.UseProblemDetails();
app.UseIpRateLimiting();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    IApiVersionDescriptionProvider provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    foreach (string groupName in provider.ApiVersionDescriptions.Select(d => d.GroupName))
    {
        options.SwaggerEndpoint($"/swagger/{groupName}/swagger.json", groupName.ToUpperInvariant());
    }
});
app.UseHsts();
app.UseHttpsRedirection();

app.UseMigrationsEndPoint();
app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints => endpoints.MapControllers());

app.MapGet("/about",
    () =>
    {
        System.Diagnostics.FileVersionInfo versionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(typeof(Program).Assembly.Location);
        return Results
            .Json(new
            {
                productVersion = versionInfo.ProductVersion,
                product = versionInfo.ProductName,
                copyright = versionInfo.LegalCopyright
            });
    });
app.MapGet("/server_time", (bool utc) =>
    Results.Json(new { time = utc ? DateTime.UtcNow.ToString("o") : DateTime.Now.ToString("o") }));

app.MapDelete("/api/reset",
    async ([FromServices] IIssueDataMigration migration) =>
    {
        await migration.ResetAndRepopultateAsync();
        return Results.StatusCode(StatusCodes.Status418ImATeapot);
    });

app.MapHealthChecks("/health",
    new HealthCheckOptions
    {
        AllowCachingResponses = false,
        ResultStatusCodes =
        {
            [HealthStatus.Healthy] = StatusCodes.Status200OK,
            [HealthStatus.Degraded] = StatusCodes.Status200OK,
            [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
        },
        ResponseWriter = WriteHealthResponse,
    });

app.Run();

static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
{
    // using second service provider to allow system.text.json to be used for the general case
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
    IServiceProvider builder = new ServiceCollection()
        .AddLogging()
        .AddMvc()
        .AddNewtonsoftJson()
        .Services
        .BuildServiceProvider();
#pragma warning restore ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'

    return builder
        .GetRequiredService<IOptions<MvcOptions>>()
        .Value
        .InputFormatters
        .OfType<NewtonsoftJsonPatchInputFormatter>()
        .First();
#pragma warning restore IDE0079 // Remove unnecessary suppression
}

static Task WriteHealthResponse(HttpContext context, HealthReport report)
{
    ISyncPolicy<string> cachePolicy = context
        .RequestServices.GetRequiredService<IReadOnlyPolicyRegistry<string>>()
        .Get<ISyncPolicy<string>>("health");

    context.Response.ContentType = "application/json; charset=utf-8";
    string healthResponse = cachePolicy
        .Execute(_ =>
        {
            var options = new JsonWriterOptions { Indented = false };

            using var memoryStream = new MemoryStream();
            using (var jsonWriter = new Utf8JsonWriter(memoryStream, options))
            {
                jsonWriter.WriteStartObject();
                jsonWriter.WriteString("status", report.Status.ToString());
                jsonWriter.WriteStartObject("results");

                foreach (KeyValuePair<string, HealthReportEntry> healthReportEntry in report.Entries)
                {
                    jsonWriter.WriteStartObject(healthReportEntry.Key.ToSnakeCase());
                    jsonWriter.WriteString("status",
                        healthReportEntry.Value.Status.ToString());
                    jsonWriter.WriteString("description",
                        healthReportEntry.Value.Description);

                    if (healthReportEntry.Value.Data.Any())
                    {
                        jsonWriter.WriteStartObject("data");
                        foreach (KeyValuePair<string, object> item in healthReportEntry.Value.Data)
                        {
                            jsonWriter.WritePropertyName(item.Key.ToSnakeCase());

                            JsonSerializer.Serialize(jsonWriter, item.Value,
                                item.Value?.GetType() ?? typeof(object));
                        }

                        jsonWriter.WriteEndObject();
                    }

                    jsonWriter.WriteEndObject();
                }

                jsonWriter.WriteEndObject();
                jsonWriter.WriteEndObject();
            }

            return Encoding.UTF8.GetString(memoryStream.ToArray());

        }, new Context("health-key"));

    return context.Response.WriteAsync(healthResponse);
}
