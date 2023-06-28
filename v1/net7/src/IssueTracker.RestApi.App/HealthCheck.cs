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

using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using TSMoreland.Text.Json.NamingStrategies;

namespace IssueTracker.RestApi.App;

public static class HealthCheck
{
    public static HealthCheckOptions GetOptions(Func<HealthCheckRegistration, bool>? predicate = null)
    {
        return new HealthCheckOptions
        {
            Predicate = predicate,
            AllowCachingResponses = false,
            ResultStatusCodes =
        {
            [HealthStatus.Healthy] = StatusCodes.Status200OK,
            [HealthStatus.Degraded] = StatusCodes.Status200OK,
            [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
        },
            ResponseWriter = WriteResponse,
        };
    }

    public static Task WriteResponse(HttpContext context, HealthReport report)
    {
#if DEBUG
        const bool authenticated = true;
#else
    bool authenticated = context.User.Identity?.IsAuthenticated == true;
#endif

        context.Response.ContentType = "application/json; charset=utf-8";
        var options = new JsonWriterOptions { Indented = false };

        using var memoryStream = new MemoryStream();
        using (var jsonWriter = new Utf8JsonWriter(memoryStream, options))
        {
            jsonWriter.WriteStartObject();
            jsonWriter.WriteString("status", report.Status.ToString());
            if (authenticated)
            {
                jsonWriter.WriteStartObject("results");

                foreach (KeyValuePair<string, HealthReportEntry> healthReportEntry in report.Entries)
                {
                    jsonWriter.WriteStartObject(healthReportEntry.Key.ToSnakeCase());

                    jsonWriter.WriteString("status",
                        healthReportEntry.Value.Status.ToString());
                    if (healthReportEntry.Value.Description is { Length: > 0 })
                    {
                        jsonWriter.WriteString("description",
                            healthReportEntry.Value.Description);
                    }

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
            }
            jsonWriter.WriteEndObject();
        }

        string healthResponse = Encoding.UTF8.GetString(memoryStream.ToArray());

        return context.Response.WriteAsync(healthResponse);
    }
}
