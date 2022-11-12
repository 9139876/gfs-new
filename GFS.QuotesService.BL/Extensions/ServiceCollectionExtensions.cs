using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GFS.QuotesService.BL.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegistryTinkoffRemoteApi(this IServiceCollection services, IConfiguration configuration)
    {
        var token = configuration.GetSection("TinkoffApiToken").Value;

        if (string.IsNullOrWhiteSpace(token))
            throw new InvalidOperationException("Tinkoff api key not specified in environment variables");
        
        services.AddInvestApiClient((_, settings) =>
        {
            settings.AccessToken = token;
        });
        
        return services;
    }
}