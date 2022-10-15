using DatabaseCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicCollection.BusinessLogic.Repositories.Auth;
using MusicCollection.BusinessLogic.Repositories.Database;
using MusicCollection.BusinessLogic.Repositories.Files.Nodes;
using MusicCollection.BusinessLogic.Repositories.Files.Roots;
using MusicCollection.BusinessLogic.Services.FilesService;
using MusicCollection.BusinessLogic.Services.UsersService;
using MusicCollection.Common.Loggers;
using MusicCollection.Common.Loggers.NLog;
using MusicCollection.Common.TagsService;

namespace ApiUtils;

public static class StartupContainerConfiguration
{
    public static void ConfigureContainer(IConfiguration configuration, IServiceCollection services)
    {
        var postgreSqlConfigurationSection = configuration.GetSection("PostgreSql");
        services.Configure<DatabaseOptions>(postgreSqlConfigurationSection);
        services.AddTransient<DbContext, DatabaseContext>();
        services.AddDbContext<DatabaseContext>(ServiceLifetime.Transient, ServiceLifetime.Transient);
        services.AddTransient<ISqlRepository<UserStorageElement>, SqlRepository<UserStorageElement>>();
        services.AddTransient<ISqlRepository<RootStorageElement>, SqlRepository<RootStorageElement>>();
        services.AddTransient<ISqlRepository<NodeStorageElement>, SqlRepository<NodeStorageElement>>();

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
    }
}