using System.IO.Compression;
using System.Reflection;
using System.Text.Json.Serialization;
using Hellang.Middleware.ProblemDetails;
using IssueTracker.App.Infrastructure;
using IssueTracker.App.OpenApi;
using IssueTracker.App.Services;
using IssueTracker.Data.Abstractions;
using IssueTracker.Middelware.SecurityHeaders;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Tcell.Agent.AspNetCore;

string? appFolder = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
if (appFolder is null)
{
    return;
}

string[] issueTrackerFiles = Directory.GetFiles(appFolder, "IssueTracker.*.dll").Select(file => Path.GetFileName(file)[..^4]).ToArray();
string issueTrackerAssemblies = string.Join(';', issueTrackerFiles);

Environment.SetEnvironmentVariable("ASPNETCORE_HOSTINGSTARTUPASSEMBLIES", issueTrackerAssemblies);


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.WebHost
    .ConfigureKestrel(kestrelServerOptions => kestrelServerOptions.AddServerHeader = false)
    .UseTcellAgent(environemnt => environemnt.IsProduction() || environemnt.IsDevelopment());

// Add services to the container.

builder.Services.AddProblemDetails();
builder.Services
    .AddControllers()
    .ConfigureApiBehaviorOptions(apiBehaviourOptions =>
    {
        apiBehaviourOptions.SuppressConsumesConstraintForFormFileParameters = true;
        apiBehaviourOptions.SuppressInferBindingSourcesForParameters = true;
        apiBehaviourOptions.SuppressMapClientErrors = true;
        apiBehaviourOptions.SuppressModelStateInvalidFilter = true;
    })
    .AddJsonOptions(jsonOptions =>
    {
        jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        jsonOptions.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        jsonOptions.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddSecurityHeaders(builder.Configuration);
builder.Services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'V");
builder.Services.AddApiVersioning(
    options =>
    {
        options.ApiVersionReader = new AggregateApiVersionReader();
        options.AssumeDefaultVersionWhenUnspecified = false;
        options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    });

builder.Services
    .AddSingleton<IConfigureOptions<SwaggerGenOptions>>(p => new SwashbuckleConfiguration(p.GetRequiredService<IApiVersionDescriptionProvider>(), p))
    .AddSwaggerGen()
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
    });

builder.Services.AddScoped<IssuesService>();

WebApplication app = builder.Build();
using (IServiceScope scope = app.Services.CreateScope())
{
    IIssueDataMigration migration =  scope.ServiceProvider.GetRequiredService<IIssueDataMigration>();
    migration.Migrate();
}

// Configure the HTTP request pipeline.
app.UseSecurityHeaders();

app.UseProblemDetails();

app.UseSwagger();

app.UseSwaggerUI();

app.UseMigrationsEndPoint();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
