using GFS.BkgWorker.Worker;
using Microsoft.Extensions.DependencyInjection;

namespace GFS.BkgWorker.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterBkgWorker(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IWorker, Worker.Worker>();
        return serviceCollection;
    }
}