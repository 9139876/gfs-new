using GFS.BackgroundWorker.Execution;
using GFS.BackgroundWorker.Models;
using Microsoft.Extensions.DependencyInjection;

namespace GFS.BackgroundWorker.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterTaskStorage<TContext>(this IServiceCollection services)
        where TContext : class, IBkgWorkerTaskContext
    {
        services
            .AddSingleton<ITasksStorage<TContext>, TasksStorage<TContext>>();

        return services;
    }
}