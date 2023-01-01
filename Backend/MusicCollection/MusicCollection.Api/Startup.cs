using ApiUtils;
using ApiUtils.ContainerConfiguration;
using ApiUtils.Middlewares;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MusicCollection.BusinessLogic.Repositories.Auth;
using MusicCollection.BusinessLogic.Repositories.Database;
using MusicCollection.BusinessLogic.Repositories.Files.Nodes;
using MusicCollection.BusinessLogic.Repositories.Files.Roots;
using MusicCollection.BusinessLogic.Services.FilesService;
using MusicCollection.BusinessLogic.Services.UsersService;
using MusicCollection.Common.Loggers.NLog;
using MusicCollection.Common.TagsService;
using SqlRepositoryBase.Configuration.Extensions;
using ILogger = MusicCollection.Common.Loggers.ILogger;

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
        
        services.ConfigureLogger()
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