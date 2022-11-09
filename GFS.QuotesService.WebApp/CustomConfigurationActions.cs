using GFS.WebApplication;
using Serilog;

namespace GFS.QuotesService.WebApp;

public class CustomConfigurationActions : ICustomConfigurationActions
{
    public void ConfigureServiceCollection(IServiceCollection services, IConfiguration configuration)
    {
        
    }

    public void ConfigureMapper(IServiceCollection services)
    {
        
    }

    public async Task ConfigureApplication(Microsoft.AspNetCore.Builder.WebApplication application, IServiceCollection services)
    {
        
    }
    
    public LoggerConfiguration CustomConfigureLogger(LoggerConfiguration lc)
    {
        return lc;
    }
}