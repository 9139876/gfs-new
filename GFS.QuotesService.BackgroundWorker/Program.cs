// See https://aka.ms/new-console-template for more information

using System.Diagnostics.CodeAnalysis;
using GFS.Common.Extensions;
using GFS.WebApplication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GFS.QuotesService.BackgroundWorker;

public static class Program
{
    public static async Task Main(string[] args)
    {
        await ProgramUtils.RunConsoleApplication<CustomConfigurationActions>(args, AppMainTask);
    }

    [SuppressMessage("ReSharper", "FunctionNeverReturns")]
    private static async Task AppMainTask(string[] args, IServiceProvider serviceProvider)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<CustomConfigurationActions>>();
        var config = serviceProvider.GetRequiredService<IConfiguration>();
        
        logger.LogInformation(config.Serialize());
        
        while (true)
        {
            await Task.Delay(1000);
            logger.LogInformation("I`am alive!");
        }
    }
}