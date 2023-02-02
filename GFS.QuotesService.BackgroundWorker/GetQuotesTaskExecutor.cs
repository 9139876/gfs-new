using GFS.QuotesService.Api.Common.Enum;
using GFS.QuotesService.BackgroundWorker.Api.Models;
using GFS.QuotesService.BL.Services;

namespace GFS.QuotesService.BackgroundWorker;

public static class GetQuotesTaskExecutor
{
    public static async Task Execute(IQuotesProviderService service, CreateTaskRequest request)
    {
        var task = request.TaskType switch
        {
            GetQuotesTaskTypeEnum.GetInitialData => service.InitialAssets(request.QuotesProviderType),
            GetQuotesTaskTypeEnum.GetHistory => service.,
            GetQuotesTaskTypeEnum.GetRealtimeQuotes => expr,
            _ => throw new ArgumentOutOfRangeException()
        };

        await task;
    }
}