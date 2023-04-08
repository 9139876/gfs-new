using GFS.BackgroundWorker.Execution;
using GFS.QuotesService.BackgroundWorker.Api.Models;
using GFS.QuotesService.BL.Services;
using GFS.QuotesService.Common.Enum;

namespace GFS.QuotesService.BackgroundWorker.Execution;

public static class WorkersManager
{
    private static readonly List<Thread> Threads = new();

    public static void Init(
        IQuotesProviderService quotesProviderService,
        ITasksStorage<QuotesServiceBkgWorkerTaskContext> tasksStorage,
        ILogger logger)
    {
        foreach (QuotesProviderTypeEnum quotesProviderType in Enum.GetValues(typeof(QuotesProviderTypeEnum)))
        {
            var executor = new TaskExecutor(quotesProviderType, quotesProviderService, tasksStorage, logger);

            var thread = new Thread(executor.Execute);
            Threads.Add(thread);
        }

        Threads.ForEach(thread => thread.Start());
    }
}
