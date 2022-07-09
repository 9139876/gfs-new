using GFS.Api.Client.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GFS.Api.Client.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterRemoteApi(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddScoped<IRemoteApiClient, RemoteApiClient>();

            return services;
        }
    }
}