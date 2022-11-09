using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace GFS.WebApplication
{
    public interface ICustomConfigurationActions
    {
        void ConfigureServiceCollection(IServiceCollection services, IConfiguration configuration);
        void ConfigureMapper(IServiceCollection services);
        Task ConfigureApplication(Microsoft.AspNetCore.Builder.WebApplication application, IServiceCollection services);
        LoggerConfiguration CustomConfigureLogger(LoggerConfiguration lc);
    }
}