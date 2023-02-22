using ApiUtils.ContainerConfiguration;
using ApiUtils.Middlewares;
using Microsoft.EntityFrameworkCore;
using MusicCollection.UserService.Repositories.Database;
using SqlRepositoryBase.Configuration.Extensions;
using TelemetryApp.Utilities.Extensions;
using TelemetryApp.Utilities.Middlewares;

namespace MusicCollection.UserService;

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
            .ConfigureTelemetryClientWithLogger("MusicCollection", "MusicCollection.UserService", telemetryApiUrl)
            .ConfigureTagsExtractor()
            .ConfigurePostgreSql()
            .ConfigureBusinessLogicRepositories()
            .ConfigureBusinessLogicServices();

        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();
        app.UseWebSockets();
        app.UseMiddleware<RequestLoggingMiddleware>();
        app.UseMiddleware<ExceptionsMiddleware>();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}