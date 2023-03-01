using System.Diagnostics.CodeAnalysis;
using GFS.QuotesService.Api.Common.Enum;
using GFS.QuotesService.BackgroundWorker.Api.Enum;
using GFS.QuotesService.BackgroundWorker.Api.Models;
using GFS.QuotesService.BL.Services;

namespace GFS.QuotesService.BackgroundWorker.Execution;

public class TaskExecutor
{
    private readonly QuotesProviderTypeEnum _quotesProviderType;
    private readonly IQuotesProviderService _quotesProviderService;
    private readonly ILogger _logger;

    public TaskExecutor(
        QuotesProviderTypeEnum quotesProviderType,
        IQuotesProviderService quotesProviderService,
        ILogger logger)
    {
        _quotesProviderType = quotesProviderType;
        _quotesProviderService = quotesProviderService;
        _logger = logger;
    }

    [SuppressMessage("ReSharper", "AccessToModifiedClosure")]
    [SuppressMessage("ReSharper", "FunctionNeverReturns")]
    public void Execute()
    {
        using var loggerScope = _logger.BeginScope("{TaskExecutorType}", $"Executor_{_quotesProviderType}");

        while (true)
        {
            while (TasksStorage.TryGetTaskForExecute(_quotesProviderType, out var task))
            {
                try
                {
                    Action action = task!.TaskType switch
                    {
                        GetQuotesTaskTypeEnum.GetInitialData => () => ExecuteInitialAssets(task),
                        GetQuotesTaskTypeEnum.GetRealtimeQuotes => throw new NotImplementedException("GetRealtimeQuotes not implemented yet"),
                        GetQuotesTaskTypeEnum.GetHistory => () => ExecuteGetHistoryQuotes(task),
                        _ => throw new ArgumentOutOfRangeException(nameof(task.TaskType))
                    };

                    action.Invoke();

                    TasksStorage.ReportOfSuccess(task.TaskId);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"Task {task!.TaskType} for {task!.QuotesProviderType} failed");
                    TasksStorage.ReportOfFail(task!.TaskId, e.Message);
                }
            }

            Thread.Sleep(1000);
        }
    }

    private void ExecuteInitialAssets(BkgWorkerTask task)
    {
        _quotesProviderService.InitialAssets(task.QuotesProviderType).Wait();
    }

    private void ExecuteGetHistoryQuotes(BkgWorkerTask task)
    {
        _quotesProviderService.GetOrUpdateHistory(task.QuotesProviderType, task.AssetId ?? throw new ArgumentNullException(nameof(task.AssetId))).Wait();
    }
}