using GFS.QuotesService.Api.Common.Enum;

namespace GFS.QuotesService.BackgroundWorker.Api.Models;

public class AddTaskRequest
{
    public QuotesProviderTypeEnum QuotesProviderType { get; set; }

    public GetQuotesTaskTypeEnum TaskType { get; set; }

    public Guid? AssetId { get; set; }
}