using GFS.ChartService.BL.Services;
using GFS.ChartService.BL.Services.Project;
using Microsoft.Extensions.DependencyInjection;

namespace GFS.ChartService.BL.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterProjectsCache(this IServiceCollection services)
    {
        services.AddSingleton<IProjectsStorage, ProjectsStorage>();
        services.AddSingleton<ISessionService, SessionService>();

        return services;
    }
}