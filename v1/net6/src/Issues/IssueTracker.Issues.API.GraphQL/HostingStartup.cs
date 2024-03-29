﻿//
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

using GraphQL;
using GraphQL.DataLoader;
using GraphQL.MicrosoftDI;
using GraphQL.Server;
using GraphQL.SystemTextJson;
using IssueTracker.Issues.API.GraphQL;
using IssueTracker.Issues.API.GraphQL.Schemas;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

[assembly: HostingStartup(typeof(HostingStartup))]

namespace IssueTracker.Issues.API.GraphQL;

/// <inheritdoc cref="IHostingStartup"/>
public sealed class HostingStartup : IHostingStartup
{
    /// <inheritdoc />
    public void Configure(IWebHostBuilder builder)
    {
        builder.ConfigureServices(Configure);
    }

    private static void Configure(WebHostBuilderContext context, IServiceCollection services) =>
        Configure(services, context.HostingEnvironment);


    /// <summary>
    /// Configure Services
    /// </summary>
    public static void Configure(IServiceCollection services, IHostEnvironment environment)
    {
        services
            .AddGraphQL(builder => builder
                .ConfigureExecutionOptions(options =>
                {
                    options.EnableMetrics = true;
                    ILogger<HostingStartup>? logger =
                        options.RequestServices?.GetRequiredService<ILogger<HostingStartup>>();
                    options.UnhandledExceptionDelegate = ctx =>
                    {
                        logger?.LogError("{Error} occurred", ctx.OriginalException.Message);
                        return Task.CompletedTask;
                    };
                })
                .AddSystemTextJson()
                .AddErrorInfoProvider(options => options.ExposeExceptionDetails = environment.IsDevelopment())
                .AddDataLoader()
                .AddSchema<IssuesSchema>()
                .AddGraphTypes(typeof(IssuesSchema).Assembly));
    }
}
