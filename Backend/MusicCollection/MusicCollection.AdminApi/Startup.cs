using ApiUtils;
using ApiUtils.ContainerConfiguration;
using ApiUtils.Middlewares;

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