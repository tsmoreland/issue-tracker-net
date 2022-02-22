using System.IO.Compression;
using System.Reflection;
using System.Runtime.Loader;
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

Assembly? entryAssembly = Assembly.GetEntryAssembly();
string? appFolder = Path.GetDirectoryName(entryAssembly?.Location);
if (appFolder is null)
{
    return;
}
string assemblyFilename = entryAssembly?.Location!;

AssemblyName[] referencedAssemblies = entryAssembly!.GetReferencedAssemblies()
    .Where(assemblyName => assemblyName.Name?.StartsWith("IssueTracker") == true)
    .ToArray();

List<string> assemblyFilenames = Directory.GetFiles(appFolder, "IssueTracker.*.dll").ToList();

List<string> unloadedAssemblyFilenames = assemblyFilenames
    .Where(filename => !string.Equals(filename, assemblyFilename, StringComparison.OrdinalIgnoreCase))
    .Select(filename => new { File = filename, Asm = AssemblyName.GetAssemblyName(filename) })
    .Where(pair => !referencedAssemblies.Any(asmName => string.Equals(asmName.Name, pair.Asm.Name, StringComparison.OrdinalIgnoreCase) && asmName.Version == pair.Asm.Version))
    .Select(pair => pair.File)
    .ToList();

unloadedAssemblyFilenames.ForEach(filename => AssemblyLoadContext.Default.LoadFromAssemblyPath(filename));

List<string> assemblyNames = assemblyFilenames
    .Select(AssemblyName.GetAssemblyName)
    .Where(assemblyName => !referencedAssemblies.Contains(assemblyName))
    .Select(assemblyName => assemblyName.Name ?? string.Empty)
    .Where(name => name is {Length: >0})
    .ToList();
assemblyNames.ForEach(assemblyName => Assembly.Load(assemblyName));

string[] hostingAssemblyNames = assemblyNames
    .Where(assemblyName => Assembly.Load(assemblyName).GetTypes().Any(t => typeof(IHostingStartup).IsAssignableFrom(t)))
    .Select(assemblyName => assemblyName.ToString())
    .ToArray();
string issueTrackerAssemblies = string.Join(';', hostingAssemblyNames);

#if bad
string[] issueTrackerFiles = assemblyFilenames.Select(file => Path.GetFileName(file)[..^4]).ToArray();
string issueTrackerAssemblies = string.Join(';', issueTrackerFiles);
#endif

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
