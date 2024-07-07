using AutoMapper;
using GFS.Common.Extensions;
using GFS.Common.Helpers;
using GFS.GrailCommon.Extensions;
using GFS.QuotesService.BL.Extensions;
using GFS.QuotesService.BL.Models;
using GFS.QuotesService.BL.Models.Adapters;
using GFS.QuotesService.BL.QuotesProviderAdapters.Abstraction;
using GFS.QuotesService.Common.Enum;
using GFS.QuotesService.DAL.Entities;

namespace GFS.QuotesService.BL.Services;

public interface IQuotesProviderService
{
    /// <summary>
    /// Получение самой первой котировки
    /// </summary>
    Task<QuoteEntity> GetFirstQuote(GetFirstQuoteRequestModel request);
    
    /// <summary>
    /// Получение списка активов
    /// </summary>
    Task<List<AssetEntity>> GetAssetsList(QuotesProviderTypeEnum quotesProviderType);
    
    /// <summary>
    /// Получение партии котировок
    /// </summary>
    Task<GetQuotesBatchResponseModel> GetQuotesBatch(GetQuotesBatchRequestModel request);
}

internal class QuotesProviderService : IQuotesProviderService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IMapper _mapper;
    public QuotesProviderService(
        IServiceProvider serviceProvider,
        IMapper mapper)
    {
        _serviceProvider = serviceProvider;
        _mapper = mapper;
    }

    public Task<QuoteEntity> GetFirstQuote(GetFirstQuoteRequestModel request)
    {
        throw new NotImplementedException();
    }

    public async Task<List<AssetEntity>> GetAssetsList(QuotesProviderTypeEnum quotesProviderType)
    {
        using var transaction = SystemTransaction.Default();

        var adapter = _serviceProvider.GetQuotesProviderAdapter(quotesProviderType);
        
        var initialModels = await adapter.GetAssetsData();

        var assets = initialModels.Select(im =>
        {
            var asset = _mapper.Map<AssetEntity>(im);
            asset.AssetInfo = _mapper.Map<AssetInfoEntity>(im);
            asset.AssetInfo.AssetId = asset.Id;
            asset.ProviderCompatibilities.Add(new AssetProviderCompatibilityEntity
            {
                AssetId = asset.Id,
                QuotesProviderType = quotesProviderType,
                IsCompatibility = true
            });

            return asset;
        }).ToList();

        return assets;
    }

    public async Task<GetQuotesBatchResponseModel> GetQuotesBatch(GetQuotesBatchRequestModel request)
    {
        var adapter = _serviceProvider.GetQuotesProviderAdapter(request.QuotesProviderType);

        var adapterRequest = new GetQuotesBatchAdapterRequestModel
        {
            Asset = request.Asset,
            TimeFrame = request.TimeFrame,
            BatchBeginningDate = request.LastQuoteDate.AddDate(request.TimeFrame, 1)
        };

        var (quoteEntitiesBatch, isLastBatch) = await GetNextQuotesBatch(adapter, adapterRequest);

        return new GetQuotesBatchResponseModel { Quotes = quoteEntitiesBatch, IsLastBatch = isLastBatch};
    }

    #region private

    private async Task<(List<QuoteEntity> quotes, bool)> GetNextQuotesBatch(IQuotesProviderAdapter adapter,
        GetQuotesBatchAdapterRequestModel adapterRequest)
    {
        var batch = await adapter.GetQuotesBatch(adapterRequest);
        var quoteEntities = _mapper.Map<List<QuoteEntity>>(batch.Quotes);

        quoteEntities.ForEach(quote =>
        {
            quote.TimeFrame = adapterRequest.TimeFrame;
            quote.AssetId = adapterRequest.Asset.Id;
            quote.QuotesProviderType = adapter.ProviderType;

            quote.Date = quote.Date.CorrectDateByTf(adapterRequest.TimeFrame);

            quote.Validate();
        });

        return (quoteEntities.ToList(), batch.IsLastBatch);
    }

    #endregion
}