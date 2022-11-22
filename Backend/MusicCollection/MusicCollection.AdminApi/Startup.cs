using ApiUtils;
using ApiUtils.ContainerConfiguration;
using ApiUtils.Middlewares;
using Microsoft.EntityFrameworkCore;
using MusicCollection.BusinessLogic.Repositories.Database;

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
        services.AddTransient<DbContext, DatabaseContext>();
        services.AddDbContext<DatabaseContext>(ServiceLifetime.Transient, ServiceLifetime.Transient);
        
        services.ConfigureLogger()
            .ConfigurePostgreSql(Configuration)
            .ConfigureTagsExtractor()
            .ConfigureLogicServices();

        services.AddCors(options =>
        {
            options.AddPolicy(CorsConfigurationName, policy =>
            {
                policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            });
        });

        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseHttpsRedirection();

        app.UseMiddleware<RequestLoggingMiddleware>();
        app.UseRouting();
        app.UseCors(CorsConfigurationName);
        app.UseWebSockets();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }

    private const string CorsConfigurationName = "AllowOrigins";
}