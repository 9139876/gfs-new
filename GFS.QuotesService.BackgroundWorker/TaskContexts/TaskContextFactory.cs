using GFS.BkgWorker.Task;
using GFS.GrailCommon.Enums;
using GFS.QuotesService.Api.Common.Enum;

namespace GFS.QuotesService.BackgroundWorker.TaskContexts;

public static class TaskContextFactory
{
    public static TaskContext CreateTaskContext(
        GetQuotesTaskTypeEnum taskType,
        QuotesProviderTypeEnum quotesProviderType,
        Guid? assetId,
        TimeFrameEnum? timeFrame)
    {
        return taskType switch
        {
            GetQuotesTaskTypeEnum.GetInitialData => CreateGetInitialDataTaskContext(quotesProviderType),
            GetQuotesTaskTypeEnum.GetHistory => CreateGetHistoryQuotesTaskContext(quotesProviderType, assetId, timeFrame),
            GetQuotesTaskTypeEnum.GetRealtimeQuotes => CreateGetRealtimeQuotesTaskContext(quotesProviderType, assetId, timeFrame),
            _ => throw new ArgumentOutOfRangeException(nameof(taskType), taskType, null)
        };
    }

    private static GetInitialDataTaskContext CreateGetInitialDataTaskContext(QuotesProviderTypeEnum quotesProviderType)
    {
        return new GetInitialDataTaskContext(quotesProviderType);
    }

    private static GetHistoryQuotesTaskContext CreateGetHistoryQuotesTaskContext(QuotesProviderTypeEnum quotesProviderType, Guid? assetId = null, TimeFrameEnum? timeFrame = null)
    {
        return new GetHistoryQuotesTaskContext(
            assetId ?? throw new InvalidOperationException($"{nameof(assetId)} is required"), 
            quotesProviderType, 
            timeFrame ?? throw new InvalidOperationException($"{nameof(timeFrame)} is required"));
    }

    private static GetRealtimeQuotesTaskContext CreateGetRealtimeQuotesTaskContext(QuotesProviderTypeEnum quotesProviderType, Guid? assetId = null, TimeFrameEnum? timeFrame = null)
    {
        return new GetRealtimeQuotesTaskContext(
            assetId ?? throw new InvalidOperationException($"{nameof(assetId)} is required"), 
            quotesProviderType, 
            timeFrame ?? throw new InvalidOperationException($"{nameof(timeFrame)} is required"));
    }
}