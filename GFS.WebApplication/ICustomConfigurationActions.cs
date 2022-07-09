using Microsoft.Extensions.DependencyInjection;

namespace GFS.WebApplication
{
    public interface ICustomConfigurationActions
    {
        void ConfigureServiceCollection(IServiceCollection services);
        void ConfigureMapper(IServiceCollection services);
        void ConfigureApplication(Microsoft.AspNetCore.Builder.WebApplication application);
    }
}