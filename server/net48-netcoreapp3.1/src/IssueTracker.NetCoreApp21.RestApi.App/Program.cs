using IssueTracker.EFCore21.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Tcell.Agent.AspNetCore;

namespace IssueTracker.NetCoreApp21.RestApi.App;

public class Program
{
    public static void Main(string[] args)
    {
        IWebHost app = CreateWebHostBuilder(args).Build();

        using (IServiceScope scope = app.Services.CreateScope())
        {
            IIssueDataMigration dataMigrator = scope.ServiceProvider.GetRequiredService<IIssueDataMigration>();
            dataMigrator.Migrate();
        }

        app.Run();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .UseTcellAgent(env => true) // usually !env.IsDevelopment(), but we want it for testing
            .UseKestrel(options => options.AddServerHeader = false)
            .UseStartup<Startup>();
}
