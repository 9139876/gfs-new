using GFS.QuotesService.Api.Common.Enum;
using GFS.QuotesService.BackgroundWorker.Api.Enum;
using GFS.QuotesService.BackgroundWorker.Api.Models;
using GFS.QuotesService.BackgroundWorker.Api.Models.RequestResponse;

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
                ? AssetIds.Select(id => new BkgWorkerTaskCreateRequest
                {
                    AssetId = id,
                    QuotesProviderType = QuotesProviderType,
                    TaskType = TaskType
                }).ToList()
                : new List<BkgWorkerTaskCreateRequest>
                {
                    new()
                    {
                        QuotesProviderType = QuotesProviderType,
                        TaskType = TaskType
                    }
                }
        };
}