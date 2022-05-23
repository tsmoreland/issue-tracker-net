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
using System.Text.Json.Serialization;
using Hellang.Middleware.ProblemDetails;
using IssueTracker.Issues.Domain.DataContracts;
using IssueTracker.Middelware.SecurityHeaders;
using IssueTracker.RestApi.App.Filters;
using IssueTracker.ServiceDiscovery;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.ResponseCompression;
using Serilog;
using Tcell.Agent.AspNetCore;

HostingStartupDiscovery
    .DiscoverUnloadedAssembliesContainingHostingStartup()
    .SetHostingStartupAssemblies();

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Host
    .UseSerilog((context, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(context.Configuration));
builder.WebHost
    .ConfigureKestrel(kestrelServerOptions => kestrelServerOptions.AddServerHeader = false)
    .UseTcellAgent(environemnt => (environemnt.IsProduction() || environemnt.IsDevelopment()) && File.Exists(Path.Combine(environemnt.ContentRootPath, "TcellAgent.config")));

// Add services to the container.

builder.Services.AddProblemDetails();

builder.Services
    .AddControllers(options => options.RespectBrowserAcceptHeader = true)
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
        jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        jsonOptions.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        jsonOptions.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddSecurityHeaders(builder.Configuration);
builder.Services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'V");
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



WebApplication app = builder.Build();
using (IServiceScope scope = app.Services.CreateScope())
{
    IIssueDataMigration migration =  scope.ServiceProvider.GetRequiredService<IIssueDataMigration>();
    await migration.MigrateAsync();
    await migration.ResetAndRepopultateAsync();
}

app.UseSecurityHeaders();
app.UseProblemDetails();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    IApiVersionDescriptionProvider provider = app.Services .GetRequiredService<IApiVersionDescriptionProvider>();
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
app.MapGet("/serverTime", (bool utc) =>
    Results.Json(new {time = utc ? DateTime.UtcNow.ToString("o") : DateTime.Now.ToString("o")}));

app.MapDelete("/api/reset",
    async ([FromServices] IIssueDataMigration migration) =>
    {
        await migration.ResetAndRepopultateAsync();
        return Results.StatusCode(StatusCodes.Status418ImATeapot);
    });

app.Run();
