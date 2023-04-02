using GFS.Common.Attributes;
using GFS.GrailCommon.Enums;
using GFS.QuotesService.Api.Common.Enum;
using GFS.QuotesService.BL.Models;

namespace GFS.QuotesService.BL.QuotesProviderAdapters.Abstraction;

public interface IQuotesProviderAdapter
{
    Task<List<InitialModel>> GetInitialData();
    bool IsNativeSupportedTimeframe(TimeFrameEnum timeFrame);
    ICollection<TimeFrameEnum> NativeSupportedTimeFrames { get; }
    QuotesProviderTypeEnum ProviderType { get; }

        /// <summary>
    /// Возвращает партию котировок и признак, есть ли еще что грузить, за указанную дату и ранее нее, идёт по истории во обратном направлении
    /// </summary>
    /// <param name="request">Модель запроса получения партии котировок</param>
    /// <returns></returns>
    Task<GetQuotesBatchResponseModel> GetQuotesBatch(GetQuotesBatchRequestModel request);
}

[IgnoreRegistration]
internal abstract class QuotesProviderAbstractAdapter : IQuotesProviderAdapter
{
    public virtual Task<List<InitialModel>> GetInitialData()
    {
        throw new NotImplementedException("У данного адаптера метод не реализован");
    }

    public bool IsNativeSupportedTimeframe(TimeFrameEnum timeFrame)
        => NativeSupportedTimeFrames.Contains(timeFrame);

    public async Task<GetQuotesBatchResponseModel> GetQuotesBatch(GetQuotesBatchRequestModel request)
    {
        if (!IsNativeSupportedTimeframe(request.TimeFrame))
            throw new InvalidOperationException($"Timeframe {request.TimeFrame} is not supported {GetType().Name}");

        return await GetQuotesBatchInternal(request);
    }
    
    protected abstract Task<GetQuotesBatchResponseModel> GetQuotesBatchInternal(GetQuotesBatchRequestModel request);

    public abstract ICollection<TimeFrameEnum> NativeSupportedTimeFrames { get; }
    
    public abstract QuotesProviderTypeEnum ProviderType { get; }
}