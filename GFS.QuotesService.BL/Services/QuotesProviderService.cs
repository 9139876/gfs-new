using AutoMapper;
using GFS.Common.Extensions;
using GFS.Common.Helpers;
using GFS.EF.Extensions;
using GFS.EF.Repository;
using GFS.GrailCommon.Extensions;
using GFS.QuotesService.Api.Common.Enum;
using GFS.QuotesService.BL.Enum;
using GFS.QuotesService.BL.Extensions;
using GFS.QuotesService.BL.Models;
using GFS.QuotesService.BL.QuotesProviderAdapters.Abstraction;
using GFS.QuotesService.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace GFS.QuotesService.BL.Services;

public interface IQuotesProviderService
{
    Task InitialAssets(QuotesProviderTypeEnum quotesProviderType);
    Task GetQuotesHistory(QuotesProviderTypeEnum quotesProviderType, Guid assetId);
}

internal class QuotesProviderService : IQuotesProviderService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IMapper _mapper;
    private readonly IDbContext _dbContext;

    public QuotesProviderService(
        IServiceProvider serviceProvider,
        IMapper mapper,
        IDbContext dbContext)
    {
        _serviceProvider = serviceProvider;
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task InitialAssets(QuotesProviderTypeEnum quotesProviderType)
    {
        using var transaction = SystemTransaction.Default();

        var adapter = _serviceProvider.GetQuotesProviderAdapter(quotesProviderType);

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

        var existingAssets = await _dbContext.GetRepository<AssetEntity>().Get().ToListAsync();
        var newAssets = assets.Except(existingAssets).ToList();

        _dbContext.GetRepository<AssetEntity>().InsertRange(newAssets);

        await _dbContext.SaveChangesAsync();
        transaction.Complete();
    }

    public async Task GetQuotesHistory(QuotesProviderTypeEnum quotesProviderType, Guid assetId)
    {
        var isInitial = !await _dbContext.GetRepository<QuoteEntity>()
            .Exist(q => q.AssetId == assetId && q.QuotesProviderType == quotesProviderType);

        var adapter = _serviceProvider.GetQuotesProviderAdapter(quotesProviderType);

        var quotes = isInitial
            ? await GetInitialQuotesHistory(adapter, assetId)
            : await RefreshQuotesHistory(adapter, assetId);

        await _dbContext.GetRepository<QuoteEntity>().BulkInsertRangeAsync(quotes.Distinct(new QuoteEntityComparerByTfAndDate()).ToList());
        await _dbContext.BulkSaveChangesAsync();
    }

    #region private

    private async Task<List<QuoteEntity>> GetInitialQuotesHistory(IQuotesProviderAdapter adapter, Guid assetId)
    {
        var quotes = new List<QuoteEntity>();

        foreach (var timeFrame in adapter.NativeSupportedTimeFrames)
        {
            var tfQuotes = new List<QuoteEntity>();

            var asset = await _dbContext.GetRepository<AssetEntity>()
                .Get(asset => asset.Id == assetId)
                .SingleOrFailAsync();

            var date = DateTime.UtcNow;

            var mustGoOn = true;

            while (mustGoOn)
            {
                var adapterRequest = new GetQuotesBatchRequestModel
                {
                    Asset = asset,
                    TimeFrame = timeFrame,
                    TimeDirection = TimeDirectionEnum.Backward,
                    BatchBeginningDate = date
                };

                var (quoteEntities, isLastBatch, nextBatchBeginningDate) = await GetNextQuotesBatch(adapter, adapterRequest);
                tfQuotes.AddRange(quoteEntities);

                mustGoOn = !isLastBatch;

                if (!isLastBatch)
                    date = nextBatchBeginningDate!.Value;
            }

            quotes.AddRange(tfQuotes);
        }

        return quotes;
    }

    private async Task<List<QuoteEntity>> RefreshQuotesHistory(IQuotesProviderAdapter adapter, Guid assetId)
    {
        var quotes = new List<QuoteEntity>();

        foreach (var timeFrame in adapter.NativeSupportedTimeFrames)
        {
            var tfQuotes = new List<QuoteEntity>();

            var asset = await _dbContext.GetRepository<AssetEntity>()
                .Get(asset => asset.Id == assetId)
                .SingleOrFailAsync();

            var lastQuoteDate = await _dbContext.GetRepository<QuoteEntity>()
                .Get(q => q.AssetId == assetId && q.QuotesProviderType == adapter.ProviderType && q.TimeFrame == timeFrame)
                .OrderBy(q => q.Date)
                .Select(q => q.Date)
                .LastAsync();

            var dbLastQuoteDate = lastQuoteDate;
            
            var mustGoOn = true;

            while (mustGoOn)
            {
                var adapterRequest = new GetQuotesBatchRequestModel
                {
                    Asset = asset,
                    TimeFrame = timeFrame,
                    TimeDirection = TimeDirectionEnum.Forward,
                    BatchBeginningDate = lastQuoteDate
                };

                var (quoteEntities, isLastBatch, nextBatchBeginningDate) = await GetNextQuotesBatch(adapter, adapterRequest);
                tfQuotes.AddRange(quoteEntities);

                mustGoOn = !isLastBatch;
                
                if (!isLastBatch)
                    lastQuoteDate = nextBatchBeginningDate!.Value;
            }

            quotes.AddRange(tfQuotes.Where(q => q.Date > dbLastQuoteDate));
        }

        return quotes;
    }

    private async Task<(List<QuoteEntity> quotes, bool isLastBatch, DateTime? nextBatchBeginningDate)> GetNextQuotesBatch(IQuotesProviderAdapter adapter, GetQuotesBatchRequestModel adapterRequest)
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

        return (quoteEntities, batch.IsLastBatch, batch.NextBatchBeginningDate);
    }

    #endregion
}