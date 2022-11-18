using GFS.QuotesService.Api.Common.Enum;

namespace GFS.QuotesService.BackgroundWorker.Api.Models;

public class GetTasksRequest
{
    public QuotesProviderTypeEnum QuotesProviderType { get; set; }
}