using GFS.BkgWorker.Abstraction;
using GFS.BkgWorker.Task;
using GFS.GrailCommon.Enums;
using GFS.QuotesService.Api.Common.Enum;
using GFS.QuotesService.Api.Enum;

namespace GFS.QuotesService.BackgroundWorker.TaskContexts;

public class GetQuotesTaskContext : TaskContext
{
    public GetQuotesTaskContext(Guid assetId, QuotesProviderTypeEnum quotesProviderType, TimeFrameEnum timeFrame)
    {
        AssetId = assetId;
        QuotesProviderType = quotesProviderType;
        TimeFrame = timeFrame;
    }

    public Guid AssetId { get; }

    public QuotesProviderTypeEnum QuotesProviderType { get; }

    public TimeFrameEnum TimeFrame { get; }

    protected override string SerializeImpl()
        => $"{AssetId}-{QuotesProviderType}-{TimeFrame}";
}