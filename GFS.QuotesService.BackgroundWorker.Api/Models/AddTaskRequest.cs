using GFS.GrailCommon.Enums;
using GFS.QuotesService.Api.Common.Enum;

namespace GFS.QuotesService.BackgroundWorker.Api.Models;

public class AddTaskRequest
{
    public QuotesProviderTypeEnum QuotesProviderType { get; set; }

    public GetQuotesTaskTypeEnum TaskType { get; set; }

    public byte Attempts { get; set; } = 3;

    public Guid? AssetId { get; set; }
    
    public TimeFrameEnum? TimeFrame { get; set; }
}