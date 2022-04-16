using IssueTracker.GrpcApi.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
ConfigureServices(builder.Services, builder.Environment);

WebApplication app = builder.Build();
ConfigurePipeline(app);

app.Run();

static void ConfigureServices(IServiceCollection services, IHostEnvironment environment)
{
    services.AddControllers();
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
    if (app.Environment.IsDevelopment())
    {
    }
    else
    {
        app.UseHsts();
        app.UseHttpsRedirection();
    }
    //app.UseMigrationsEndPoint();
 
    app.UseRouting();
    app.UseCors();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapGrpcServices();
}

