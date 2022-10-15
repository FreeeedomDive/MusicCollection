using ApiUtils;
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
        StartupContainerConfiguration.ConfigureContainer(Configuration, services);

        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseHttpsRedirection();

        app.UseMiddleware<RequestLoggingMiddleware>();
        app.UseRouting();
        app.UseWebSockets();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}