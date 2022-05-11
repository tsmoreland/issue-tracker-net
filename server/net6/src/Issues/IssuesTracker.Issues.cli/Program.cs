using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;
using IssueTracker.Issues.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(ConfigureServices)
    .Build();


using IServiceScope scope = host.Services.CreateScope();

ILoggerFactory loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
IssuesDbContext dbContext = scope.ServiceProvider.GetRequiredService<IssuesDbContext>();
ILogger logger = loggerFactory.CreateLogger<IssuesDbContext>();

await dbContext.Database.MigrateAsync();

Issue epic = new("APP",1, "Epic1", "...") { Type = IssueType.Epic };

try
{
    dbContext.Issues.Add(epic);
    dbContext.SaveChanges();
}
catch (Exception ex)
{
    logger.LogError("{Exception}", ex.ToString());
}

Issue story = new("APP",2, "Story1", "...") { Type = IssueType.Story, EpidId = epic.Id };
try
{
    dbContext.Issues.Add(story);
    dbContext.SaveChanges();
}
catch (Exception ex)
{
    logger.LogError("{Exception}", ex.ToString());
}


static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
{
    services
        .AddDbContext<IssuesDbContext>();

}

