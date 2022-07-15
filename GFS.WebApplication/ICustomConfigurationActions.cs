using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GFS.WebApplication
{
    public interface ICustomConfigurationActions
    {
        void ConfigureServiceCollection(IServiceCollection services, IConfiguration configuration);
        void ConfigureMapper(IServiceCollection services);
        Task ConfigureApplication(Microsoft.AspNetCore.Builder.WebApplication application);
    }
}