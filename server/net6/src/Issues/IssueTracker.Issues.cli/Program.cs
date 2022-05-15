using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;
using IssueTracker.Issues.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

using Version1 = IssueTracker.Issues.API.Version1.Abstractions;
using Version2 = IssueTracker.Issues.API.Version2.Abstractions;


#if ASPNETCORE
using IssueTracker.ServiceDiscovery;
// we're not but it shows  what it would look like if we were
HostingStartupDiscovery
    .DiscoverUnloadedAssembliesContainingHostingStartup()
    .SetHostingStartupAssemblies();

#endif


IHostBuilder hostBuilder = Host.CreateDefaultBuilder(args)
    .UseSerilog((context, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(context.Configuration));

#if !ASPNETCORE
hostBuilder
    .ConfigureServices(services =>
    {
        HostingStartup.Configure(services);
        IssueTracker.Issues.API.HostingStartup.Configure(services);
    });
#endif

IHost host = hostBuilder.Build();

await InitializeDatabase(host.Services);

using IServiceScope scope = host.Services.CreateScope();
if (args.Any())
{
    switch (args[0].ToLowerInvariant())
    {
        case "dbcontext":
            VerifyAddToContext(scope);
            break;
        case "repository":
            await VerifyRepository(scope);
            break;
        case "apiv1":
            await VerifyApiV1(scope);
            break;
        case "apiv2":
            break;
    }
}


await VerifyApiV1(scope);


static async Task InitializeDatabase(IServiceProvider provider)
{
    using IServiceScope scope = provider.CreateScope();
    IssuesDbContext dbContext = scope.ServiceProvider.GetRequiredService<IssuesDbContext>();

    await dbContext.Database.EnsureDeletedAsync();
    await dbContext.Database.MigrateAsync();
}

static void VerifyAddToContext(IServiceScope scope)
{
    IssuesDbContext dbContext = scope.ServiceProvider.GetRequiredService<IssuesDbContext>();
    ILogger logger = scope.ServiceProvider
        .GetRequiredService<ILoggerFactory>()
        .CreateLogger<Program>();

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

    Issue story = new("APP",2, "Story1", "...") { Type = IssueType.Story, EpicId = epic.Id };
    try
    {
        dbContext.Issues.Add(story);
        dbContext.SaveChanges();
    }
    catch (Exception ex)
    {
        logger.LogError("{Exception}", ex.ToString());
    }
}

static async Task VerifyRepository(IServiceScope scope)
{
    IIssueRepository repository = scope.ServiceProvider.GetRequiredService<IIssueRepository>();
    Issue epic = new("APP",1, "Epic1", "...") { Type = IssueType.Epic };

    epic = repository.Add(epic);

    Issue story = new("APP",2, "Story1", "...") { Type = IssueType.Story, EpicId = epic.Id };
    story = repository.Add(story);

    await repository.UnitOfWork.SaveEntitiesAsync();

    DisplayIssue(epic);
    DisplayIssue(story);
}

static async Task VerifyApiV1(IServiceScope scope)
{
    IMediator mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

    Version1.DataTransferObjects.IssueDto epic =
        await mediator.Send(new Version1.Commands.CreateIssueCommand("app", "sample epic", "first epic", Priority.High));

    Version1.DataTransferObjects.IssueDto story =
        await mediator.Send(new Version1.Commands.CreateIssueCommand("app", "sample story", "first story", Priority.Medium));

    DisplayIssueDto(epic);
    DisplayIssueDto(story);
}

static void DisplayIssue(Issue issue)
{
    Console.WriteLine($"{issue.Id} {issue.Title}");
}
static void DisplayIssueDto(Version1.DataTransferObjects.IssueDto issue)
{
    Console.WriteLine($"{issue.Id} {issue.Title}");
}
