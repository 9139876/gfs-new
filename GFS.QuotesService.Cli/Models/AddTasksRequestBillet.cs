using GFS.QuotesService.BackgroundWorker.Api.Enum;
using GFS.QuotesService.BackgroundWorker.Api.Models;
using GFS.QuotesService.BackgroundWorker.Api.Models.RequestResponse;
using GFS.QuotesService.Common.Enum;

namespace GFS.QuotesService.Cli.Models;

public class AddTasksRequestBillet
{
    public List<Guid> AssetIds { get; set; } = new();

    public QuotesProviderTypeEnum QuotesProviderType { get; set; }

    public GetQuotesTaskTypeEnum TaskType { get; set; }

    public AddTasksRequest CreateRequest()
        => new AddTasksRequest
        {
            Tasks = AssetIds.Any() && TaskType != GetQuotesTaskTypeEnum.GetInitialData
                ? AssetIds.Select(id => new QuotesServiceBkgWorkerTaskContext
                {
                    AssetId = id,
                    QuotesProviderType = QuotesProviderType,
                    TaskType = TaskType
                }).ToList()
                : new List<QuotesServiceBkgWorkerTaskContext>
                {
                    new()
                    {
                        QuotesProviderType = QuotesProviderType,
                        TaskType = TaskType
                    }
                }
        };
}