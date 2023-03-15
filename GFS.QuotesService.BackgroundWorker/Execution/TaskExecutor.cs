using System.Diagnostics.CodeAnalysis;
using GFS.BackgroundWorker.Execution;
using GFS.BackgroundWorker.Models;
using GFS.QuotesService.Api.Common.Enum;
using GFS.QuotesService.BackgroundWorker.Api.Enum;
using GFS.QuotesService.BackgroundWorker.Api.Models;
using GFS.QuotesService.BL.Services;

namespace GFS.QuotesService.BackgroundWorker.Execution;

public class TaskExecutor
{
    private readonly QuotesProviderTypeEnum _quotesProviderType;
    private readonly IQuotesProviderService _quotesProviderService;
    private readonly ITasksStorage<QuotesServiceBkgWorkerTaskContext> _tasksStorage;
    private readonly ILogger _logger;

    public TaskExecutor(
        QuotesProviderTypeEnum quotesProviderType,
        IQuotesProviderService quotesProviderService,
        ITasksStorage<QuotesServiceBkgWorkerTaskContext> tasksStorage,
        ILogger logger)
    {
        _quotesProviderType = quotesProviderType;
        _quotesProviderService = quotesProviderService;
        _tasksStorage = tasksStorage;
        _logger = logger;
    }

    [SuppressMessage("ReSharper", "AccessToModifiedClosure")]
    [SuppressMessage("ReSharper", "FunctionNeverReturns")]
    public void Execute()
    {
        using var loggerScope = _logger.BeginScope("{TaskExecutorType}", $"Executor_{_quotesProviderType}");

        while (true)
        {
            while (_tasksStorage.TryGetTaskForExecute(ctx => ctx.QuotesProviderType == _quotesProviderType, out var task))
            {
                try
                {
                    Action action = task!.Context.TaskType switch
                    {
                        GetQuotesTaskTypeEnum.GetInitialData => () => ExecuteInitialAssets(task),
                        GetQuotesTaskTypeEnum.GetRealtimeQuotes => throw new NotImplementedException("GetRealtimeQuotes not implemented yet"),
                        GetQuotesTaskTypeEnum.GetHistory => () => ExecuteGetHistoryQuotes(task),
                        _ => throw new ArgumentOutOfRangeException(nameof(task.Context.TaskType))
                    };

                    action.Invoke();

                    _tasksStorage.ReportOfSuccess(task.TaskId);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Task {TaskType} for {QuotesProviderType} failed", task!.Context.TaskType, task.Context.QuotesProviderType);
                    _tasksStorage.ReportOfFail(task.TaskId, e.Message);
                }
            }

            Thread.Sleep(1000);
        }
    }

    private void ExecuteInitialAssets(BkgWorkerTask<QuotesServiceBkgWorkerTaskContext> task)
    {
        _quotesProviderService.InitialAssets(task.Context.QuotesProviderType).Wait();
    }

    private void ExecuteGetHistoryQuotes(BkgWorkerTask<QuotesServiceBkgWorkerTaskContext> task)
    {
        _quotesProviderService.GetOrUpdateHistory(task.Context.QuotesProviderType, task.Context.AssetId ?? throw new ArgumentNullException(nameof(task.Context.AssetId))).Wait();
    }
}