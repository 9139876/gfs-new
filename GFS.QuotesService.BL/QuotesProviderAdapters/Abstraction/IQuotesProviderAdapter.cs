using GFS.Common.Attributes;
using GFS.GrailCommon.Enums;
using GFS.GrailCommon.Extensions;
using GFS.GrailCommon.Models;
using GFS.QuotesService.BL.Models;
using GFS.QuotesService.DAL.Entities;

namespace GFS.QuotesService.BL.QuotesProviderAdapters.Abstraction;

public interface IQuotesProviderAdapter
{
    Task<List<InitialModel>> GetInitialData();
    bool IsNativeSupportedTimeframe(TimeFrameEnum timeFrame);
    TimeFrameEnum[] NativeSupportedTimeFrames { get; }
    Task<IEnumerable<QuoteModel>> GetQuotesBatch(AssetEntity asset, TimeFrameEnum timeFrame, DateTime? lastQuoteDate);
}

[IgnoreRegistration]
public abstract class QuotesProviderAbstractAdapter : IQuotesProviderAdapter
{
    public virtual Task<List<InitialModel>> GetInitialData()
    {
        throw new NotImplementedException("У данного адаптера метод не реализован");
    }

    public bool IsNativeSupportedTimeframe(TimeFrameEnum timeFrame)
        => NativeSupportedTimeFrames.Contains(timeFrame);

    public async Task<IEnumerable<QuoteModel>> GetQuotesBatch(AssetEntity asset, TimeFrameEnum timeFrame, DateTime? lastQuoteDate)
    {
        if (!IsNativeSupportedTimeframe(timeFrame))
            throw new InvalidOperationException($"Timeframe {timeFrame} is not supported {GetType().Name}");

        return lastQuoteDate.HasValue
            ? await GetQuotesBatchInternal(asset, timeFrame, lastQuoteDate.Value)
            : new[] { await GetFirstQuote(asset, timeFrame) };
    }

    protected async Task<QuoteModel> GetFirstQuote(AssetEntity asset, TimeFrameEnum timeFrame)
    {
        var left = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var right = DateTime.UtcNow;
        QuoteModel? result = null;

        while ((right - left).Days > 1)
        {
            var median = DateWithTimeFrameExtensions.GetMedian(left, right);
            var earlyQuote = (await GetQuotesBatchInternal(asset, timeFrame, median)).FirstOrDefault();

            if (earlyQuote != null)
            {
                result = earlyQuote;
                right = median;
            }
            else
            {
                left = median;
            }
        }

        return result ?? throw new InvalidOperationException();
    }

    protected abstract Task<IEnumerable<QuoteModel>> GetQuotesBatchInternal(AssetEntity asset, TimeFrameEnum timeFrame, DateTime lastQuoteDate);

    public abstract TimeFrameEnum[] NativeSupportedTimeFrames { get; }
}