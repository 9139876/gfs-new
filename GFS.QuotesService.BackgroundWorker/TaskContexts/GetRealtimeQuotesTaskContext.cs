using GFS.BkgWorker.Task;
using GFS.GrailCommon.Enums;
using GFS.QuotesService.Api.Common.Enum;

namespace GFS.QuotesService.BackgroundWorker.TaskContexts;

public class GetRealtimeQuotesTaskContext : TaskContext
{
    public GetRealtimeQuotesTaskContext(Guid assetId, QuotesProviderTypeEnum quotesProviderType, TimeFrameEnum timeFrame)
    {
        AssetId = assetId;
        QuotesProviderType = quotesProviderType;
        TimeFrame = timeFrame;
    }

    public Guid AssetId { get; }

    public QuotesProviderTypeEnum QuotesProviderType { get; }

    public TimeFrameEnum TimeFrame { get; }
    
    public override Task<bool> DoWork(IServiceProvider serviceProvider)
    {
        throw new NotImplementedException();
        // return true;
    }

    protected override string SerializeImpl()
        => $"{AssetId}-{QuotesProviderType}-{TimeFrame}";
}