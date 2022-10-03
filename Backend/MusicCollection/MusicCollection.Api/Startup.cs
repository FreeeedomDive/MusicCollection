using Loggers.NLog;
using Microsoft.OpenApi.Models;
using MusicCollection.BusinessLogic.Repositories.Auth;
using MusicCollection.BusinessLogic.Repositories.Database;
using MusicCollection.BusinessLogic.Repositories.Files.Nodes;
using MusicCollection.BusinessLogic.Repositories.Files.Roots;
using MusicCollection.BusinessLogic.Services.FilesService;
using MusicCollection.BusinessLogic.Services.UsersService;
using MusicCollection.Middlewares;
using TagExtractor;
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
        services.AddDbContext<DatabaseContext>(ServiceLifetime.Transient, ServiceLifetime.Transient);

        // add default logger
        services.AddSingleton<ILogger>(NLogLogger.Build("Default"));
        
        // add services without dependencies
        services.AddTransient<ITagsExtractor, TagsExtractor>();

        // add repositories
        services.AddTransient<IUsersRepository, UsersRepository>();
        services.AddTransient<IRootsRepository, RootsRepository>();
        services.AddTransient<INodesRepository, NodesRepository>();

        // add logic services
        services.AddTransient<IUsersService, UsersService>();
        services.AddTransient<IFilesService, FilesService>();

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