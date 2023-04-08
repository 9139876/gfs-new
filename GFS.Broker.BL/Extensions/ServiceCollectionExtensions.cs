using GFS.Broker.BL.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GFS.Broker.BL.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterBlServices(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var dateServiceImplementationType = configuration.GetSection("").Value switch
        {
            "Test" => typeof(TestDateService),
            "Real" => typeof(RealDateService),
            _ => throw new ArgumentOutOfRangeException() //ToDo error message - specify settings parameter
        };

        var dealServiceImplementationType = configuration.GetSection("").Value switch
        {
            "Test" => typeof(TestDealService),
            "Real" => typeof(RealDealService),
            _ => throw new ArgumentOutOfRangeException() //ToDo error message - specify settings parameter
        };

        serviceCollection.AddSingleton<ISettingsService, SettingsService>();
        serviceCollection.AddSingleton(typeof(ICurrentDateService), dateServiceImplementationType);
        serviceCollection.AddScoped(typeof(IDealService), dealServiceImplementationType);

        return serviceCollection;
    }
}