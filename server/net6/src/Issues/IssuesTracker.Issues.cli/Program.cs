using IssueTracker.Issues.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(ConfigureServices)
    .Build();


using IServiceScope scope = host.Services.CreateScope();

IssuesDbContext dbContext = scope.ServiceProvider.GetRequiredService<IssuesDbContext>();

await dbContext.Database.MigrateAsync();


static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
{
    services
        .AddDbContext<IssuesDbContext>();

}

