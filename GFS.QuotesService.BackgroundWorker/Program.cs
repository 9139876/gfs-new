using System.Diagnostics.CodeAnalysis;
using GFS.BackgroundWorker;
using GFS.QuotesService.BackgroundWorker.Workers;

namespace GFS.QuotesService.BackgroundWorker;

public static class Program
{
    public static async Task Main(string[] args)
    {
        await ProgramUtils.RunConsoleApplication<CustomConfigurationActions>(args, AppMainTask);
    }

    [SuppressMessage("ReSharper", "FunctionNeverReturns")]
    private static Task[] AppMainTask(string[] args, IServiceProvider serviceProvider)
    {
        return new[]
        {
            new UpdateQuotesWorker(serviceProvider).DoWork(),
            new UpdateAssetsListWorker(serviceProvider).DoWork()
        };
    }
}