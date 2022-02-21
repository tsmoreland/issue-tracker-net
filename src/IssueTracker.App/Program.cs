using System.IO.Compression;
using System.Text.Json.Serialization;
using Hellang.Middleware.ProblemDetails;
using IssueTracker.App.Configuration;
using IssueTracker.App.Data;
using IssueTracker.App.Extensions;
using IssueTracker.App.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.WebHost
    .ConfigureKestrel(kestrelServerOptions => kestrelServerOptions.AddServerHeader = false);

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

builder.Services.AddDbContext<ApplicationDbContext>(optionsLifetime: ServiceLifetime.Singleton);
builder.Services.AddScoped<IssueRepository>();

WebApplication app = builder.Build();
using (IServiceScope scope = app.Services.CreateScope())
{
    ApplicationDbContext dbContext =  scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
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
