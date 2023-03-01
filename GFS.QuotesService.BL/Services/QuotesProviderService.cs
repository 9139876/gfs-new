using AutoMapper;
using GFS.Common.Helpers;
using GFS.EF.Extensions;
using GFS.EF.Repository;
using GFS.GrailCommon.Enums;
using GFS.QuotesService.Api.Common.Enum;
using GFS.QuotesService.BL.Extensions;
using GFS.QuotesService.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace GFS.QuotesService.BL.Services;

public interface IQuotesProviderService
{
    Task InitialAssets(QuotesProviderTypeEnum quotesProviderType);
    Task GetOrUpdateHistory(QuotesProviderTypeEnum quotesProviderType, Guid assetId);
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

        var assetRepository = _dbContext.GetRepository<AssetEntity>();
        var adapter = _serviceProvider.GetQuotesProviderAdapter(quotesProviderType);

        var initialModels = await adapter.GetInitialData();

        var assets = initialModels.Select(im =>
        {
            var asset = _mapper.Map<AssetEntity>(im);
            asset.AssetInfo = _mapper.Map<AssetInfoEntity>(im);
            asset.AssetInfo.AssetId = asset.Id;
            return asset;
        }).ToList();

        var existing = await assetRepository.Get().ToListAsync();

        var newAssets = assets.Except(existing).ToList();

        assetRepository.InsertRange(newAssets);

        var assetProviderCompatibilities = newAssets.Select(asset => new AssetProviderCompatibilityEntity
        {
            AssetId = asset.Id,
            QuotesProviderType = quotesProviderType,
            IsCompatibility = true
        });

        _dbContext.GetRepository<AssetProviderCompatibilityEntity>().InsertRange(assetProviderCompatibilities);

        await _dbContext.SaveChangesAsync();
        transaction.Complete();
    }

    public async Task GetOrUpdateHistory(QuotesProviderTypeEnum quotesProviderType, Guid assetId)
    {
        var adapter = _serviceProvider.GetQuotesProviderAdapter(quotesProviderType);

        foreach (var timeFrame in adapter.NativeSupportedTimeFrames)
        {
            var lastQuote = await _dbContext.GetRepository<QuoteEntity>()
                .Get(q => q.AssetId == assetId && q.QuotesProviderType == quotesProviderType && q.TimeFrame == timeFrame)
                .OrderBy(q => q.Date)
                .LastOrDefaultAsync();

            var lastDate = lastQuote?.Date;

            while (true)
            {
                var lastBatchDate = await GetAndSaveNextQuotesBatch(quotesProviderType, assetId, timeFrame, lastDate);

                if (lastBatchDate.HasValue && (!lastDate.HasValue || lastDate < lastBatchDate))
                    lastDate = lastBatchDate;
                else
                    break;
            }
        }
    }

    #region private

    private async Task<DateTime?> GetAndSaveNextQuotesBatch(QuotesProviderTypeEnum quotesProviderType, Guid assetId, TimeFrameEnum timeFrame, DateTime? lastQuoteDate)
    {
        var adapter = _serviceProvider.GetQuotesProviderAdapter(quotesProviderType);

        if (!adapter.IsNativeSupportedTimeframe(timeFrame))
            throw new InvalidOperationException($"Timeframe {timeFrame} is not supported on {quotesProviderType}");

        var asset = await _dbContext.GetRepository<AssetEntity>()
            .Get(asset => asset.Id == assetId)
            .SingleOrFailAsync();

        var batch = await adapter.GetQuotesBatch(asset, timeFrame, lastQuoteDate);
        var quoteEntities = _mapper.Map<List<QuoteEntity>>(batch);
        quoteEntities.ForEach(quote =>
        {
            quote.TimeFrame = timeFrame;
            quote.AssetId = assetId;
            quote.QuotesProviderType = quotesProviderType;
        });

        var existingQuotesDates = await _dbContext.GetRepository<QuoteEntity>()
            .Get(q => q.AssetId == assetId
                      && q.QuotesProviderType == quotesProviderType
                      && q.TimeFrame == timeFrame
                      && quoteEntities.Select(qe => qe.Date).Contains(q.Date))
            .Select(q => q.Date)
            .ToListAsync();

        _dbContext.GetRepository<QuoteEntity>().InsertRange(quoteEntities.Where(qe => !existingQuotesDates.Contains(qe.Date)));
        await _dbContext.SaveChangesAsync();

        return quoteEntities.LastOrDefault()?.Date;
    }

    #endregion
}