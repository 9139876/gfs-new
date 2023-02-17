using GFS.QuotesService.Api.Common.Enum;
using GFS.QuotesService.BL.Services;

namespace GFS.QuotesService.BackgroundWorker.Execution;

public static class WorkersManager
{
    private static readonly List<Thread> Threads = new();

    public static void Init(IServiceProvider serviceProvider)
    {
        foreach (QuotesProviderTypeEnum quotesProviderType in Enum.GetValues(typeof(QuotesProviderTypeEnum)))
        {
            var executor = new TaskExecutor(quotesProviderType, serviceProvider.GetRequiredService<IQuotesProviderService>(), serviceProvider.GetRequiredService<ILogger>());
            var thread = new Thread(executor.Execute);
            Threads.Add(thread);
        }

        Threads.ForEach(thread => thread.Start());
    }
}