using GFS.Common.Attributes;
using GFS.Common.Exceptions;
using GFS.GrailCommon.Enums;
using GFS.QuotesService.BL.Models;
using GFS.QuotesService.BL.Models.Adapters;
using GFS.QuotesService.Common.Enum;

namespace GFS.QuotesService.BL.QuotesProviderAdapters.Abstraction;

internal interface IQuotesProviderAdapter
{
    Task<List<AssetModel>> GetAssetsData();

    ICollection<TimeFrameEnum> NativeSupportedTimeFrames { get; }

    QuotesProviderTypeEnum ProviderType { get; }

    Task<DateTime> GetFirstQuoteDate(GetFirstQuoteAdapterRequestModel adapterRequest); 
    
    /// <summary>
    /// Возвращает партию котировок и признак, есть ли еще что грузить, за указанную дату и ранее нее, идёт по истории во обратном направлении
    /// </summary>
    /// <param name="request">Модель запроса получения партии котировок</param>
    /// <returns></returns>
    Task<GetQuotesBatchAdapterResponseModel> GetQuotesBatch(GetQuotesBatchAdapterRequestModel request);
}

[IgnoreRegistration]
internal abstract class QuotesProviderAbstractAdapter : IQuotesProviderAdapter
{
    public virtual Task<List<AssetModel>> GetAssetsData()
    {
        throw new NotImplementedYetException("У данного адаптера метод не реализован");
    }

    public virtual Task<DateTime> GetFirstQuoteDate(GetFirstQuoteAdapterRequestModel adapterRequest)
    {
        throw new NotImplementedYetException("У данного адаптера метод не реализован");
    }

    public async Task<GetQuotesBatchAdapterResponseModel> GetQuotesBatch(GetQuotesBatchAdapterRequestModel request)
    {
        if (!IsNativeSupportedTimeframe(request.TimeFrame))
            throw new InvalidOperationException($"Timeframe {request.TimeFrame} is not supported {GetType().Name}");

        return await GetQuotesBatchInternal(request);
    }

    private bool IsNativeSupportedTimeframe(TimeFrameEnum timeFrame)
        => NativeSupportedTimeFrames.Contains(timeFrame);
    
    protected abstract Task<GetQuotesBatchAdapterResponseModel> GetQuotesBatchInternal(GetQuotesBatchAdapterRequestModel request);

    public abstract ICollection<TimeFrameEnum> NativeSupportedTimeFrames { get; }

    public abstract QuotesProviderTypeEnum ProviderType { get; }
}