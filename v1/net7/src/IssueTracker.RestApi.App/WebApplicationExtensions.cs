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

using System.Threading.RateLimiting;
using IssueTracker.Issues.Domain.DataContracts;
using IssueTracker.Middelware.SecurityHeaders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Writers;
using Swashbuckle.AspNetCore.Swagger;

namespace IssueTracker.RestApi.App;

public static class WebApplicationExtensions
{
    public static async Task MigrateAsync(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        IIssueDataMigration migration = scope.ServiceProvider.GetRequiredService<IIssueDataMigration>();
        await migration.MigrateAsync();
        await migration.ResetAndRepopultateAsync();
    }

    public static async Task GenerateOpenApiDocument(this WebApplication app, string filename, string version, string hostname = "https://localhost")
    {
        ArgumentNullException.ThrowIfNull(app);

        using IServiceScope scope = app.Services.CreateScope();
        ISwaggerProvider swaggerProvider = scope.ServiceProvider.GetRequiredService<ISwaggerProvider>();
        OpenApiDocument document = swaggerProvider.GetSwagger(version, hostname);

        await using StringWriter stringWriter = new();
        OpenApiJsonWriter writer = new(stringWriter);
        document.SerializeAsV3(writer);
        await File.WriteAllTextAsync(filename, stringWriter.ToString());
    }

    public static void Configure(this WebApplication app)
    {
        app.UseSecurityHeaders();
        app.UseExceptionHandler();
        app.UseStatusCodePages();

        app
            .UseRateLimiter(new RateLimiterOptions
            {
                RejectionStatusCode = StatusCodes.Status429TooManyRequests,
                OnRejected = static (context, _) =>
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    return new ValueTask();
                }
            }.AddConcurrencyLimiter(policyName: "concurrent-limit",
                options =>
                {
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    options.PermitLimit = 50;
                    options.QueueLimit = 50;
                })
            .AddFixedWindowLimiter("fixed-limit",
                options =>
                {
                    options.Window = TimeSpan.FromMinutes(1);
                    options.PermitLimit = 50;

                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    options.QueueLimit = 50;
                }));

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

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

        app.MapControllers().RequireRateLimiting("fixed-limit");

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

        app
            .MapHealthChecks("/health", HealthCheck.GetOptions())
            .AllowAnonymous();

        app
            .MapHealthChecks("/health/ready", HealthCheck.GetOptions(registration => registration.Tags.Contains("ready")))
            .AllowAnonymous();
    }
}
