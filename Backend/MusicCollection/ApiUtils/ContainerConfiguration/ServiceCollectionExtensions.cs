using BackgroundTasksDaemon;
using BackgroundTasksDaemon.Builder;
using BackgroundTasksDaemon.Storage;
using Microsoft.Extensions.DependencyInjection;
using MusicCollection.BusinessLogic.Repositories;
using MusicCollection.BusinessLogic.Services;
using MusicCollection.Common.Loggers;
using MusicCollection.Common.Loggers.NLog;
using MusicCollection.Common.TagsService;
using SqlRepositoryBase.Core.Models;
using SqlRepositoryBase.Core.Repository;

namespace ApiUtils.ContainerConfiguration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureLogger(this IServiceCollection services)
    {
        services.AddSingleton<ILogger>(NLogLogger.Build("Default"));

        return services;
    }

    public static IServiceCollection ConfigureBusinessLogicRepositories(this IServiceCollection services)
    {
        var allTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).ToArray();

        var sqlStorageElementTypes = allTypes
            .Where(p => typeof(SqlStorageElement).IsAssignableFrom(p))
            .Where(p => p != typeof(SqlStorageElement));
        foreach (var sqlStorageElementType in sqlStorageElementTypes)
        {
            var genericSqlRepositoryInterfaceType = typeof(ISqlRepository<>).MakeGenericType(sqlStorageElementType);
            var genericSqlRepositoryImplementationType = typeof(SqlRepository<>).MakeGenericType(sqlStorageElementType);
            services.AddTransient(genericSqlRepositoryInterfaceType, genericSqlRepositoryImplementationType);
        }

        var repositories = allTypes.Where(type =>
            type.IsInterface
            && typeof(IMusicCollectionRepository).IsAssignableFrom(type)
            && type != typeof(IMusicCollectionRepository)
        );
        foreach (var repository in repositories)
        {
            var implementation = allTypes.First(type => repository.IsAssignableFrom(type) && type != repository);
            services.AddTransient(repository, implementation);
        }

        return services;
    }

    public static IServiceCollection ConfigureBusinessLogicServices(this IServiceCollection services)
    {
        var allTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).ToArray();

        var logicServices = allTypes.Where(type =>
            type.IsInterface
            && typeof(IMusicCollectionLogicService).IsAssignableFrom(type)
            && type != typeof(IMusicCollectionLogicService)
        );
        foreach (var service in logicServices)
        {
            var implementation = allTypes.First(type => service.IsAssignableFrom(type) && type != service);
            services.AddTransient(service, implementation);
        }

        return services;
    }

    public static IServiceCollection ConfigureTagsExtractor(this IServiceCollection services)
    {
        services.AddTransient<ITagsExtractor, TagsExtractor>();

        return services;
    }

    public static IServiceCollection ConfigureTasksWorker(this IServiceCollection services)
    {
        services.AddTransient<IBackgroundTaskBuilder, BackgroundTaskBuilder>();
        services.AddSingleton<IBackgroundTasksStorage, BackgroundTasksStorage>();
        services.AddTransient<ITasksDaemon, TasksDaemon>();

        return services;
    }
}