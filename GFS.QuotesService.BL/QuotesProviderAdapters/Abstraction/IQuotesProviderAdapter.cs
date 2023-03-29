using GFS.Common.Attributes;
using GFS.GrailCommon.Enums;
using GFS.GrailCommon.Models;
using GFS.QuotesService.BL.Models;
using GFS.QuotesService.DAL.Entities;

namespace GFS.QuotesService.BL.QuotesProviderAdapters.Abstraction;

public interface IQuotesProviderAdapter
{
    Task<List<InitialModel>> GetInitialData();
    bool IsNativeSupportedTimeframe(TimeFrameEnum timeFrame);
    ICollection<TimeFrameEnum> NativeSupportedTimeFrames { get; }
    
    /// <summary>
    /// Возвращает партию котировок, за указанную дату и ранее нее, идёт по истории во обратном направлении
    /// </summary>
    /// <param name="asset">Инструмент</param>
    /// <param name="timeFrame">Таймфрейм</param>
    /// <param name="batchEndDate">Дата последней желаемой котировки в партии</param>
    /// <returns></returns>
    Task<IEnumerable<QuoteModel>> GetQuotesBatch(AssetEntity asset, TimeFrameEnum timeFrame, DateTime batchEndDate);
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

    public async Task<IEnumerable<QuoteModel>> GetQuotesBatch(AssetEntity asset, TimeFrameEnum timeFrame, DateTime batchEndDate)
    {
        if (!IsNativeSupportedTimeframe(timeFrame))
            throw new InvalidOperationException($"Timeframe {timeFrame} is not supported {GetType().Name}");

        return await GetQuotesBatchInternal(asset, timeFrame, batchEndDate);
    }
    
    protected abstract Task<IEnumerable<QuoteModel>> GetQuotesBatchInternal(AssetEntity asset, TimeFrameEnum timeFrame, DateTime batchEndDate);

    public abstract ICollection<TimeFrameEnum> NativeSupportedTimeFrames { get; }
}