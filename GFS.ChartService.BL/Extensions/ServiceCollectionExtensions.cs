using GFS.ChartService.BL.Services.Project;
using Microsoft.Extensions.DependencyInjection;

namespace GFS.ChartService.BL.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterProjectsCache(this IServiceCollection services)
    {
        services.AddSingleton<IProjectsCache, ProjectsCache>();

        return services;
    }
}