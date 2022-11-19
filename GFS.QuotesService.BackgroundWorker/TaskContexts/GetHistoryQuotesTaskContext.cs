using GFS.BkgWorker.Task;
using GFS.GrailCommon.Enums;
using GFS.GrailCommon.Extensions;
using GFS.QuotesService.Api.Common.Enum;
using GFS.QuotesService.BL.Services;

namespace GFS.QuotesService.BackgroundWorker.TaskContexts;

public class GetHistoryQuotesTaskContext : TaskContext
{
    public GetHistoryQuotesTaskContext(Guid assetId, QuotesProviderTypeEnum quotesProviderType, TimeFrameEnum timeFrame)
    {
        AssetId = assetId;
        QuotesProviderType = quotesProviderType;
        TimeFrame = timeFrame;
    }

    public Guid AssetId { get; }

    public QuotesProviderTypeEnum QuotesProviderType { get; }

    public TimeFrameEnum TimeFrame { get; }

    public override async Task<bool> DoWork(IServiceProvider serviceProvider)
    {
        var quotesProviderService = serviceProvider.GetRequiredService<IQuotesProviderService>();
        var lastQuoteDate = await quotesProviderService.GetAndSaveNextQuotesBatch(QuotesProviderType, AssetId, TimeFrame);
        return NeedContinue(lastQuoteDate);
    }

    private bool NeedContinue(DateTime lastQuoteDate)
    {
        return DateWithTimeFrameExtensions.GetPossibleEndDate(TimeFrame, DateTime.UtcNow).AddDays(1) > lastQuoteDate;
    }
    
    protected override string SerializeImpl()
        => $"{AssetId}-{QuotesProviderType}-{TimeFrame}";
}