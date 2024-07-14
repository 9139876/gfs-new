using GFS.Common.Attributes;
using GFS.Common.Exceptions;
using GFS.GrailCommon.Enums;
using GFS.QuotesService.Api.Enum;
using GFS.QuotesService.BL.Models;
using GFS.QuotesService.BL.Models.Adapters;

namespace GFS.QuotesService.BL.QuotesProviderAdapters.Abstraction;

internal interface IQuotesProviderAdapter
{
    /// <summary>
    /// Получение списка активов
    /// </summary>
    Task<List<AssetModel>> GetAssetsData();

    /// <summary>
    /// Список поддерживаемых адаптером таймфреймов
    /// </summary>
    ICollection<TimeFrameEnum> NativeSupportedTimeFrames { get; }

    /// <summary>
    /// Тип провайдера котировок
    /// </summary>
    QuotesProviderTypeEnum ProviderType { get; }

    /// <summary>
    /// Получение самой первой котировки
    /// </summary>
    Task<DateTime> GetFirstQuoteDate(GetFirstQuoteDateAdapterRequestModel dateAdapterRequest); 
    
    /// <summary>
    /// Возвращает партию котировок и признак, есть ли еще что грузить
    /// </summary>
    Task<GetQuotesBatchAdapterResponseModel> GetQuotesBatch(GetQuotesDateBatchAdapterRequestModel request);
}

[IgnoreRegistration]
internal abstract class QuotesProviderAbstractAdapter : IQuotesProviderAdapter
{
    public virtual Task<List<AssetModel>> GetAssetsData()
    {
        throw new NotImplementedYetException("У данного адаптера метод не реализован");
    }

    public virtual Task<DateTime> GetFirstQuoteDate(GetFirstQuoteDateAdapterRequestModel dateAdapterRequest)
    {
        throw new NotImplementedYetException("У данного адаптера метод не реализован");
    }

    public async Task<GetQuotesBatchAdapterResponseModel> GetQuotesBatch(GetQuotesDateBatchAdapterRequestModel request)
    {
        if (!IsNativeSupportedTimeframe(request.TimeFrame))
            throw new InvalidOperationException($"Timeframe {request.TimeFrame} is not supported {GetType().Name}");

        return await GetQuotesBatchInternal(request);
    }

    private bool IsNativeSupportedTimeframe(TimeFrameEnum timeFrame)
        => NativeSupportedTimeFrames.Contains(timeFrame);
    
    protected abstract Task<GetQuotesBatchAdapterResponseModel> GetQuotesBatchInternal(GetQuotesDateBatchAdapterRequestModel request);

    public abstract ICollection<TimeFrameEnum> NativeSupportedTimeFrames { get; }

    public abstract QuotesProviderTypeEnum ProviderType { get; }
}