using ApiUtils.ContainerConfiguration;
using BackgroundTasksDaemon;
using Microsoft.EntityFrameworkCore;
using MusicCollection.BusinessLogic.Repositories.Database;
using SqlRepositoryBase.Configuration.Extensions;
using TelemetryApp.Utilities.Extensions;
using TelemetryApp.Utilities.Middlewares;

namespace MusicCollection.AdminApi;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        var postgreSqlConfigurationSection = Configuration.GetSection("PostgreSql");
        services.Configure<DatabaseOptions>(postgreSqlConfigurationSection);
        services.AddDbContext<DatabaseContext>(ServiceLifetime.Transient, ServiceLifetime.Transient);
        services.AddTransient<DbContext, DatabaseContext>();

        var telemetryApiUrl = Configuration.GetSection("TelemetryApp").GetSection("ApiUrl").Value ?? throw new InvalidOperationException("TelemetryApp.Api url is not configured");
        services
            .ConfigureTelemetryClientWithLogger("MusicCollection", "MusicCollection.AdminApi", telemetryApiUrl)
            .ConfigurePostgreSql()
            .ConfigureBusinessLogicRepositories()
            .ConfigureTagsExtractor()
            .ConfigureBusinessLogicServices()
            .ConfigureTasksWorker();

        services.AddCors(options =>
        {
            options.AddPolicy(CorsConfigurationName,
                policy => { policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
        });

        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseMiddleware<RequestLoggingMiddleware>();
        app.UseRouting();
        app.UseCors(CorsConfigurationName);
        app.UseWebSockets();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        var taskDaemon = app.ApplicationServices.GetService<ITasksDaemon>();
        taskDaemon?.Start();
    }

    private const string CorsConfigurationName = "AllowOrigins";
}