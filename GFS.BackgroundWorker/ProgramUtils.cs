using GFS.WebApplication;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace GFS.BackgroundWorker;

public static class ProgramUtils
{
    public static async Task RunConsoleApplication<TCustomConfigurationActions>(string[] args, Func<string[], IServiceProvider, Task[]> appMainTask)
        where TCustomConfigurationActions : ConsoleCustomConfigurationActionsAbstract, new()
    {
        var customConfigurationActions = new TCustomConfigurationActions();

        using var host = Host.CreateDefaultBuilder(args)
            .ConfigureHostConfiguration(_ => { })
            .ConfigureServices(customConfigurationActions.ConfigureServiceCollection)
            .UseSerilog((_, lc) => { customConfigurationActions.CustomConfigureLogger(lc); })
            .Build();

        await customConfigurationActions.ConfigureApplication(host.Services);

        await Task.WhenAll(appMainTask(args, host.Services));
    }
}