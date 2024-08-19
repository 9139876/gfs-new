using AutoMapper;
using GFS.Common.Extensions;
using GFS.GrailCommon.Extensions;
using GFS.QuotesService.Api.Enum;
using GFS.QuotesService.BL.Extensions;
using GFS.QuotesService.BL.Models;
using GFS.QuotesService.BL.Models.Adapters;
using GFS.QuotesService.BL.QuotesProviderAdapters.Abstraction;
using GFS.QuotesService.DAL.Entities;

namespace GFS.QuotesService.BL.Services;

public interface IQuotesProviderService
{
    /// <summary>
    /// Получение самой первой котировки
    /// </summary>
    Task<DateTime> GetFirstQuoteDate(GetFirstQuoteRequestModel request);

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

    public async Task<DateTime> GetFirstQuoteDate(GetFirstQuoteRequestModel request)
    {
        var adapter = _serviceProvider.GetQuotesProviderAdapter(request.QuotesProviderType);
        return await adapter.GetFirstQuoteDate(new GetFirstQuoteDateAdapterRequestModel { Asset = request.Asset, TimeFrame = request.TimeFrame });
    }

    public async Task<List<AssetEntity>> GetAssetsList(QuotesProviderTypeEnum quotesProviderType)
    {
        var adapter = _serviceProvider.GetQuotesProviderAdapter(quotesProviderType);

        var initialModels = await adapter.GetAssetsData();

        var assets = initialModels.Select(im =>
        {
            var asset = _mapper.Map<AssetEntity>(im);
            asset.AssetInfo = _mapper.Map<AssetInfoEntity>(im);
            asset.AssetInfo.AssetId = asset.Id;
            asset.ProviderCompatibilities.AddRange(adapter.NativeSupportedTimeFrames.Select(tf =>
                new AssetProviderCompatibilityEntity
                {
                    AssetId = asset.Id,
                    QuotesProviderType = quotesProviderType,
                    TimeFrame = tf
                }));

            return asset;
        }).ToList();

        return assets;
    }

    public async Task<GetQuotesBatchResponseModel> GetQuotesBatch(GetQuotesBatchRequestModel request)
    {
        var adapter = _serviceProvider.GetQuotesProviderAdapter(request.QuotesProviderType);

        var adapterRequest = new GetQuotesDateBatchAdapterRequestModel
        {
            Asset = request.Asset,
            TimeFrame = request.TimeFrame,
            BatchBeginningDate = request.LastQuoteDate.AddDate(request.TimeFrame, 1)
        };

        return await GetNextQuotesBatch(adapter, adapterRequest);
    }

    #region private

    private async Task<GetQuotesBatchResponseModel> GetNextQuotesBatch(IQuotesProviderAdapter adapter, GetQuotesDateBatchAdapterRequestModel adapterRequest)
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

        return new GetQuotesBatchResponseModel(quoteEntities.ToList(), batch.IsLastBatch, batch.NextBatchBeginningDate);
    }

    #endregion
}