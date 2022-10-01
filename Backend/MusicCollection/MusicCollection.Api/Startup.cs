using Loggers;
using Loggers.NLog;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.OpenApi.Models;
using MusicCollection.BusinessLogic.Repositories.Auth;
using MusicCollection.BusinessLogic.Repositories.Database;
using MusicCollection.BusinessLogic.Repositories.Files;
using MusicCollection.BusinessLogic.Services.FilesService;
using MusicCollection.BusinessLogic.Services.UsersService;
using MusicCollection.Middlewares;
using ILogger = Loggers.ILogger;

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
        services.AddDbContext<DatabaseContext>(ServiceLifetime.Singleton, ServiceLifetime.Singleton);

        // add default logger
        services.AddSingleton<ILogger>(NLogLogger.Build("Default"));

        // add repositories
        services.AddTransient<IUsersRepository, UsersRepository>();
        services.AddTransient<IRootsRepository, RootsRepository>();
        services.AddTransient<INodesRepository, NodesRepository>();

        // add services
        services.AddTransient<IUsersService, UsersService>();
        services.AddTransient<IFilesService, FilesService>();

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
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