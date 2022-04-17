using System.IO.Compression;
using Hellang.Middleware.ProblemDetails;
using IssueTracker.Data.Abstractions;
using IssueTracker.GrpcApi.Services;
using IssueTracker.Middelware.SecurityHeaders;
using IssueTracker.ServiceDiscovery;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Serilog;

HostingStartupDiscovery
    .DiscoverUnloadedAssembliesContainingHostingStartup()
    .SetHostingStartupAssemblies();

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Host
    .UseSerilog((context, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(context.Configuration));
builder.WebHost
    .ConfigureKestrel(kestrelServerOptions =>
    {
        kestrelServerOptions.AddServerHeader = false;
        kestrelServerOptions.ConfigureEndpointDefaults(endpointDefaults =>
            endpointDefaults.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2);
        kestrelServerOptions.ConfigureHttpsDefaults(httpsOptions =>
            httpsOptions.SslProtocols =  System.Security.Authentication.SslProtocols.Tls11 | System.Security.Authentication.SslProtocols.Tls12);
    });
ConfigureServices(builder.Services, builder.Environment, builder.Configuration);

WebApplication app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    IIssueDataMigration migration =  scope.ServiceProvider.GetRequiredService<IIssueDataMigration>();
    migration.Migrate();
}

ConfigurePipeline(app);

app.Run();

static void ConfigureServices(IServiceCollection services, IHostEnvironment environment, IConfiguration configuration)
{
    services.AddProblemDetails();
    services
        .AddSecurityHeaders(configuration)
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
        })
        .AddControllers();

    if (environment.IsDevelopment())
    {
        services.AddGrpc(options => options.EnableDetailedErrors = true);
    }
    else
    {
        services.AddGrpc();
    }

}

static void ConfigurePipeline(WebApplication app)
{
    //app.UseSecurityHeaders();
    //app.UseProblemDetails();

    if (app.Environment.IsDevelopment())
    {
    }
    else
    {
        //app.UseHsts();
        //app.UseHttpsRedirection();
    }
    //app.UseMigrationsEndPoint();
 
    app.UseRouting();
    app.UseCors();
    app.UseAuthentication();
    app.UseAuthorization();

    app
        .UseEndpoints(endpoints =>
        {
            endpoints.MapGrpcServices();
            endpoints.MapGet("/", () => new { error = "REST not supported, use GRPC client" });
        });
}

