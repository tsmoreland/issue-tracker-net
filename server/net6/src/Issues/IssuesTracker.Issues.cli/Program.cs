using IssueTracker.Issues.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;

IConfiguration configuration = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .AddJsonFile("appsettings.json")
    .Build();

IServiceProvider provider = ConfigureServices(new ServiceCollection(), configuration)
    .BuildServiceProvider();

using IServiceScope scope = provider.CreateScope();

IssuesDbContext dbContext = scope.ServiceProvider.GetRequiredService<IssuesDbContext>();

await dbContext.Database.MigrateAsync();


static IServiceCollection ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services
        .AddSingleton<ILoggerFactory>(new LoggerFactory())
        .AddSingleton<IHostEnvironment>(BuildHostEnvironment())
        .AddSingleton<IConfiguration>(configuration)
        .AddDbContext<IssuesDbContext>();


    return services;
}

static IHostEnvironment BuildHostEnvironment()
{
    string root = Path.GetDirectoryName(typeof(Program).Assembly.Location) ?? Directory.GetCurrentDirectory();

    HostingEnvironment hostingEnvironment = new()
    {
        ApplicationName = typeof(Program).Assembly.GetName().Name,
        EnvironmentName = Environments.Production,
        ContentRootPath = root,
    };

    hostingEnvironment.ContentRootFileProvider = new PhysicalFileProvider(root);
    return hostingEnvironment;
}
