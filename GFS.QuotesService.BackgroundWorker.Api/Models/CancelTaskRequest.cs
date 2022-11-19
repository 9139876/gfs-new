using GFS.GrailCommon.Enums;
using GFS.QuotesService.Api.Common.Enum;

namespace GFS.QuotesService.BackgroundWorker.Api.Models;

public class CancelTaskRequest
{
    public QuotesProviderTypeEnum QuotesProviderType { get; set; }

    public GetQuotesTaskTypeEnum TaskType { get; set; }

    public Guid? AssetId { get; set; }

    public TimeFrameEnum? TimeFrame { get; set; }
}