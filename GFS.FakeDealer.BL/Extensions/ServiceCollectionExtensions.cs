using GFS.FakeDealer.BL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GFS.FakeDealer.BL.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterBlServices(this IServiceCollection services)
    {
        services
            .AddSingleton<ISettingsService, SettingsService>()
            .AddScoped<IDealService, DealService>();

        return services;
    }
}