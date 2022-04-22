using System.IO.Compression;
using System.Security.Cryptography.X509Certificates;
using Hellang.Middleware.ProblemDetails;
using IssueTracker.Data.Abstractions;
using IssueTracker.GrpcApi.App;
using IssueTracker.GrpcApi.Services;
using IssueTracker.Middelware.SecurityHeaders;
using IssueTracker.ServiceDiscovery;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Serilog;

HostingStartupDiscovery
    .DiscoverUnloadedAssembliesContainingHostingStartup()
    .SetHostingStartupAssemblies();

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Host
    .UseSerilog((context, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(context.Configuration));
builder.WebHost.ConfigureKestrel(ConfigureKestrel);
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
    options.ConfigureEndpointDefaults(endpointDefaults =>
        endpointDefaults.Protocols = HttpProtocols.Http2);
    options.ConfigureHttpsDefaults(httpsDefaults =>
        httpsDefaults.ClientCertificateMode = Microsoft.AspNetCore.Server.Kestrel.Https.ClientCertificateMode.AllowCertificate);
}

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
        .AddCors(options =>
            options.AddPolicy("AllowAll", builder =>
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()))
        .AddControllers();

    services
        .AddAuthentication()
        .AddJwtBearer(options => options.TokenValidationParameters = new AnonymousTokenValidationParameters(configuration))
        .AddCertificate(options =>
        {
            options.AllowedCertificateTypes = CertificateTypes.All;
            options.RevocationMode = X509RevocationMode.NoCheck; // allow self-signed, not ideal for production but *maybe* if we're limiting by address and have a precise enough check on the cert

            options
                .Events = new CertificateAuthenticationEvents
                {
                    OnCertificateValidated = context =>
                    {
                        // insert certificate checks here
                        //if (context.ClientCertificate... if same-host then we can probably verify the private key matches
                        context.Success();
                        return Task.CompletedTask;
                    }
                };
        });

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
    app.UseSecurityHeaders();
    app.UseProblemDetails();

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
    app.UseAuthentication();
    app.UseAuthorization();

    app.UseGrpcWeb();
    app.MapGrpcServices();

    app.MapGet("/", () => new { error = "REST not supported, use GRPC client" });
}
