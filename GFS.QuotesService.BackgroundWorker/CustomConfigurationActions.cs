using AutoMapper;
using GFS.Common.Extensions;
using GFS.EF.Extensions;
using GFS.QuotesService.DAL;
using GFS.WebApplication;
using Serilog;

namespace GFS.QuotesService.BackgroundWorker;

public class CustomConfigurationActions : ICustomConfigurationActions
{
    public void ConfigureServiceCollection(IServiceCollection services, IConfiguration configuration)
    {
        services
            .RegisterDbContext<QuotesServiceDbContext>(configuration.GetConnectionString("DefaultConnection"))
            .RegisterAssemblyServicesByMember<BL.PlaceboRegistration>();
    }

    public void ConfigureMapper(IServiceCollection services)
    {
        services.AddAutoMapper(expr => expr.AddProfile(new MappingProfile()), typeof(CustomConfigurationActions));
    }

    public async Task ConfigureApplication(Microsoft.AspNetCore.Builder.WebApplication application, IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider(); 
        
        await serviceProvider.MigrateDatabaseAsync<QuotesServiceDbContext>();
        
        WorkersManager.Init(serviceProvider);
    }

    public LoggerConfiguration CustomConfigureLogger(LoggerConfiguration lc)
    {
        return lc;
    }
    
    private class MappingProfile : Profile
    {
    }
}