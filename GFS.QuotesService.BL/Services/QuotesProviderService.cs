using AutoMapper;
using GFS.Common.Extensions;
using GFS.Common.Helpers;
using GFS.EF.Extensions;
using GFS.EF.Repository;
using GFS.GrailCommon.Extensions;
using GFS.QuotesService.BL.Extensions;
using GFS.QuotesService.BL.Models;
using GFS.QuotesService.BL.QuotesProviderAdapters.Abstraction;
using GFS.QuotesService.Common.Enum;
using GFS.QuotesService.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GFS.QuotesService.BL.Services;

public interface IQuotesProviderService
{
    Task InitialAssets(QuotesProviderTypeEnum quotesProviderType, Action<string?> updateSubState);
    Task GetQuotesHistory(QuotesProviderTypeEnum quotesProviderType, Guid assetId, Action<string?> updateSubState);
    Task<GetQuotesBatchResponseModel2> GetQuotesBatch(GetQuotesBatchRequestModel2 request);
}

internal class QuotesProviderService : IQuotesProviderService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IMapper _mapper;
    private readonly ILogger<QuotesProviderService> _logger;
    private readonly IDbContext _dbContext;

    public QuotesProviderService(
        IServiceProvider serviceProvider,
        IMapper mapper,
        ILogger<QuotesProviderService> logger,
        IDbContext dbContext)
    {
        _serviceProvider = serviceProvider;
        _mapper = mapper;
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task InitialAssets(QuotesProviderTypeEnum quotesProviderType, Action<string?> updateSubState)
    {
        using var transaction = SystemTransaction.Default();

        var adapter = _serviceProvider.GetQuotesProviderAdapter(quotesProviderType);

        updateSubState("Получение данных");
        var initialModels = await adapter.GetInitialData();

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

        updateSubState("Данные получены, обработка");

        var existingAssets = await _dbContext.GetRepository<AssetEntity>().Get().ToListAsync();
        var newAssets = assets.Except(existingAssets).ToList();

        updateSubState("Сохранение в БД");
        _dbContext.GetRepository<AssetEntity>().InsertRange(newAssets);
        await _dbContext.SaveChangesAsync();
        transaction.Complete();
        updateSubState("Готово");
    }

    public async Task<GetQuotesBatchResponseModel2> GetQuotesBatch(GetQuotesBatchRequestModel2 request)
    {
        var adapter = _serviceProvider.GetQuotesProviderAdapter(request.QuotesProviderType);

        var asset = await _dbContext.GetRepository<AssetEntity>()
            .Get(asset => asset.Id == request.AssetId)
            .SingleOrFailAsync();

        var adapterRequest = new GetQuotesBatchRequestModel
        {
            Asset = asset,
            TimeFrame = request.TimeFrame,
            BatchBeginningDate = request.LastQuoteDate
        };

        var (quoteEntitiesBatch, isLastBatch, _) = await GetNextQuotesBatch(adapter, adapterRequest);

        return new GetQuotesBatchResponseModel2 { Quotes = quoteEntitiesBatch, IsLastBatch = isLastBatch };
    }

    public async Task GetQuotesHistory(QuotesProviderTypeEnum quotesProviderType, Guid assetId, Action<string?> updateSubState)
    {
        var adapter = _serviceProvider.GetQuotesProviderAdapter(quotesProviderType);

        var asset = await _dbContext.GetRepository<AssetEntity>()
            .Get(asset => asset.Id == assetId)
            .SingleOrFailAsync();

        var firstQuoteDates = await Task.WhenAll(adapter.NativeSupportedTimeFrames.Select(async tf => new
        {
            TimeFrame = tf,
            LastDate = DateWithTimeFrameExtensions.GetMaxDate(
                await _dbContext.GetRepository<QuoteEntity>()
                    .Get(q => q.AssetId == assetId && q.QuotesProviderType == quotesProviderType)
                    .Select(q => q.Date)
                    .OrderBy(d => d)
                    .LastOrDefaultAsync(),
                await adapter.GetFirstQuoteDate(new GetQuotesRequestModel { Asset = asset, TimeFrame = tf }))
        }));

        var quoteEntities = new List<QuoteEntity>();

        try
        {
            foreach (var item in firstQuoteDates)
            {
                updateSubState($"Получение данных {item.TimeFrame}");

                var date = item.LastDate;

                var mustGoOn = true;

                while (mustGoOn)
                {
                    var adapterRequest = new GetQuotesBatchRequestModel
                    {
                        Asset = asset,
                        TimeFrame = item.TimeFrame,
                        BatchBeginningDate = date
                    };

                    var (quoteEntitiesBatch, isLastBatch, nextBatchBeginningDate) = await GetNextQuotesBatch(adapter, adapterRequest);

                    quoteEntities.AddRange(quoteEntitiesBatch);

                    mustGoOn = !isLastBatch;

                    if (!isLastBatch)
                        date = nextBatchBeginningDate!.Value;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        finally
        {
            await _dbContext.GetRepository<QuoteEntity>().BulkInsertRangeAsync(quoteEntities.Distinct(new QuoteEntityComparerByTfAndDate()).ToList());
            await _dbContext.BulkSaveChangesAsync();

            updateSubState("Готово");
        }
    }

    #region private

    private async Task<(List<QuoteEntity> quotes, bool isLastBatch, DateTime? nextBatchBeginningDate)> GetNextQuotesBatch(IQuotesProviderAdapter adapter,
        GetQuotesBatchRequestModel adapterRequest)
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

        return (quoteEntities.ToList(), batch.IsLastBatch, batch.NextBatchBeginningDate);
    }

    #endregion
}