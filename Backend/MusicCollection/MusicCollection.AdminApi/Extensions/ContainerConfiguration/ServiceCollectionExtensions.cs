using BackgroundTasksDaemon;
using BackgroundTasksDaemon.Builder;
using BackgroundTasksDaemon.Storage;

namespace MusicCollection.AdminApi.Extensions.ContainerConfiguration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureTasksWorker(this IServiceCollection services)
    {
        services.AddTransient<IBackgroundTaskBuilder, BackgroundTaskBuilder>();
        services.AddSingleton<IBackgroundTasksStorage, BackgroundTasksStorage>();
        services.AddTransient<ITasksDaemon, TasksDaemon>();

        return services;
    }
}