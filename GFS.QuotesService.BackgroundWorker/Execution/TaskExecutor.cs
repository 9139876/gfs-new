using GFS.BackgroundWorker.Execution;
using GFS.BackgroundWorker.Models;
using GFS.QuotesService.Api.Common.Enum;
using GFS.QuotesService.BackgroundWorker.Api.Enum;
using GFS.QuotesService.BackgroundWorker.Api.Models;
using GFS.QuotesService.BL.Services;

namespace GFS.QuotesService.BackgroundWorker.Execution;

public class TaskExecutor : AbstractTaskExecutor<QuotesServiceBkgWorkerTaskContext>
{
    private readonly QuotesProviderTypeEnum _quotesProviderType;
    private readonly IQuotesProviderService _quotesProviderService;
    private readonly ILogger _logger;

    public TaskExecutor(
        QuotesProviderTypeEnum quotesProviderType,
        IQuotesProviderService quotesProviderService,
        ITasksStorage<QuotesServiceBkgWorkerTaskContext> tasksStorage,
        ILogger logger) : base(tasksStorage, logger)
    {
        _quotesProviderType = quotesProviderType;
        _quotesProviderService = quotesProviderService;
        _logger = logger;
    }

    protected override Predicate<QuotesServiceBkgWorkerTaskContext> TasksSelector
        => ctx => ctx.QuotesProviderType == _quotesProviderType;

    protected override IDisposable LoggerScope
        => _logger.BeginScope("{ExecutorNameSpace}, {TaskExecutorType}", GetType().Namespace, $"Executor_{_quotesProviderType}");

    protected override Action<Exception, BkgWorkerTask<QuotesServiceBkgWorkerTaskContext>> LogErrorMessage
        => (e, task) => _logger.LogError(e, "Task {TaskType} id {TaskId} for {QuotesProviderType} failed", task.Context.TaskType, task.TaskId, task.Context.QuotesProviderType);

    protected override void ExecuteInternal(QuotesServiceBkgWorkerTaskContext ctx)
    {
        Action action = ctx.TaskType switch
        {
            GetQuotesTaskTypeEnum.GetInitialData => () => ExecuteInitialAssets(ctx),
            GetQuotesTaskTypeEnum.GetRealtimeQuotes => throw new NotImplementedException("GetRealtimeQuotes not implemented yet"),
            GetQuotesTaskTypeEnum.GetHistory => () => ExecuteGetHistoryQuotes(ctx),
            _ => throw new ArgumentOutOfRangeException(nameof(ctx.TaskType))
        };

        action.Invoke();
    }

    private void ExecuteInitialAssets(QuotesServiceBkgWorkerTaskContext ctx)
    {
        _quotesProviderService.InitialAssets(ctx.QuotesProviderType).Wait();
    }

    private void ExecuteGetHistoryQuotes(QuotesServiceBkgWorkerTaskContext ctx)
    {
        _quotesProviderService.GetOrUpdateHistory(ctx.QuotesProviderType, ctx.AssetId ?? throw new ArgumentNullException(nameof(ctx.AssetId))).Wait();
    }
}