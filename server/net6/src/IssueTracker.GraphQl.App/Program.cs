using System.IO.Compression;
using GraphQL;
using GraphQL.MicrosoftDI;
using GraphQL.DataLoader;
using GraphQL.Execution;
using GraphQL.Instrumentation;
using GraphQL.Server;
using GraphQL.Server.Authorization.AspNetCore;
using GraphQL.Server.Transports.AspNetCore;
using GraphQL.Server.Ui.Altair;
using GraphQL.Server.Ui.GraphiQL;
using GraphQL.Server.Ui.Playground;
using GraphQL.Server.Ui.Voyager;
using GraphQL.SystemTextJson;
using IssueTracker.Data.Abstractions;
using IssueTracker.GraphQl.App.Profiles;
using IssueTracker.GraphQl.App.Services;
using IssueTracker.Middelware.SecurityHeaders;
using IssueTracker.ServiceDiscovery;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Serilog;

HostingStartupDiscovery
    .DiscoverUnloadedAssembliesContainingHostingStartup()
    .SetHostingStartupAssemblies();

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(ConfigureKestrel);
builder.Host
    .UseSerilog((context, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(context.Configuration));

ConfigureServices(builder.Services, builder.Environment, builder.Configuration);

WebApplication app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    IIssueDataMigration migration =  scope.ServiceProvider.GetRequiredService<IIssueDataMigration>();
    migration.Migrate();
}

ConfigurePipeline(app);
app.Run();

static void ConfigureKestrel(WebHostBuilderContext context, KestrelServerOptions options)
{
    options.Configure(context.Configuration.GetSection("Kestrel")); // maybe null check this ?
    options.AddServerHeader = false;
#if ENABLE_CLIENT_CERT_AUTH
    options.ConfigureHttpsDefaults(httpsDefaults =>
        httpsDefaults.ClientCertificateMode = Microsoft.AspNetCore.Server.Kestrel.Https.ClientCertificateMode.AllowCertificate);
#endif
}
static void ConfigureServices(IServiceCollection services, IHostEnvironment environment, IConfiguration configuration)
{
    services
        .AddAutoMapper(typeof(MappingProfile).Assembly)
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
        .AddCors(options =>
            options.AddPolicy("AllowAll", builder =>
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()))
        .AddControllers();

    services.AddGraphQL(builder =>
        builder
            .AddHttpMiddleware<IssueTrackerSchema>()
            .AddWebSocketsHttpMiddleware<IssueTrackerSchema>()
            .AddSchema<IssueTrackerSchema>()
            .ConfigureExecutionOptions(options =>
            {
                options.EnableMetrics = true;
                ILogger<Program>? logger = options.RequestServices?.GetRequiredService<ILogger<Program>>();
                options.UnhandledExceptionDelegate = ctx =>
                {
                    logger?.LogError("{Error} occurred", ctx.OriginalException.Message);
                    return Task.CompletedTask;
                };
            })
            .AddSystemTextJson()
            .AddErrorInfoProvider(options => options.ExposeExceptionStackTrace = environment.IsDevelopment())
            .AddWebSockets()
            .AddDataLoader()
            .AddGraphTypes(typeof(IssueTrackerSchema).Assembly));
}

static void ConfigurePipeline(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseMigrationsEndPoint();
    }
    else
    {
        app.UseHsts();
        app.UseHttpsRedirection();
    }

    app.UseRouting();
    app.UseCors();
    app.UseAuthorization();

    // Debugging note - current failure is due to not finding scoped service IIssueRepository when building IssuesType

#if USE_WEBSOCKETS
    app.UseWebSockets();
    app.UseGraphQLWebSockets<IssueTrackerSchema>();
#endif
    app.UseGraphQL<IssueTrackerSchema>();
    app.UseGraphQLPlayground(new PlaygroundOptions());
}
