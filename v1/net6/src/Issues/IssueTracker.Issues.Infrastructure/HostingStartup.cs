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

using IssueTracker.Issues.Domain.DataContracts;
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;
using IssueTracker.Issues.Infrastructure.Configurations;
using IssueTracker.Issues.Infrastructure.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(IssueTracker.Issues.Infrastructure.HostingStartup))]

namespace IssueTracker.Issues.Infrastructure;

public sealed class HostingStartup : IHostingStartup
{
    /// <inheritdoc />
    public void Configure(IWebHostBuilder builder)
    {
        builder.ConfigureServices(Configure);
    }

    public static void Configure(IServiceCollection services)
    {
        // separated to highlight that it's not a common entry while the others would be
        services.AddTransient<IIssueDataMigration, IssueDataMigration>();

        services
            .AddDbContext<IssuesDbContext>(optionsLifetime: ServiceLifetime.Singleton)
            .AddDbContextFactory<IssuesDbContext>()
            .AddSingleton<IModelConfiguration, SqliteModelConfiguration>()
            .AddScoped<IIssueRepository, IssueRepository>();
    }
}
