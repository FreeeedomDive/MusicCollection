using ApiUtils.ContainerConfiguration;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MusicCollection.BusinessLogic.Repositories.Database;
using SqlRepositoryBase.Configuration.Extensions;
using TelemetryApp.Utilities.Extensions;
using TelemetryApp.Utilities.Middlewares;

namespace MusicCollection;

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
        
        var telemetryApiUrl = Configuration.GetSection("TelemetryApp").GetSection("ApiUrl").Value ?? throw new InvalidOperationException("TelemetryApp.Api url is not configured");
        services
            .ConfigureTelemetryClientWithLogger("MusicCollection", "MusicCollection.Api", telemetryApiUrl)
            .ConfigureTagsExtractor()
            .ConfigurePostgreSql()
            .ConfigureBusinessLogicRepositories()
            .ConfigureBusinessLogicServices();

        services.AddControllers();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "MusicCollection API", Version = "v1" });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MusicCollection API v1"));
        }

        app.UseHttpsRedirection();

        app.UseRouting();
        app.UseWebSockets();
        app.UseMiddleware<RequestLoggingMiddleware>();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}