using AutoMapper;
using GFS.Common.Extensions;
using GFS.Common.Helpers;
using GFS.EF.Extensions;
using GFS.EF.Repository;
using GFS.GrailCommon.Enums;
using GFS.GrailCommon.Extensions;
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

    public async Task GetOrUpdateHistory(QuotesProviderTypeEnum quotesProviderType, Guid assetId)
    {
        var adapter = _serviceProvider.GetQuotesProviderAdapter(quotesProviderType);

        foreach (var timeFrame in adapter.NativeSupportedTimeFrames)
        {
            var prevBatchSmallestDate = DateTime.UtcNow;

            var lastQuoteDate = await _dbContext.GetRepository<QuoteEntity>()
                .Get(q => q.AssetId == assetId && q.QuotesProviderType == quotesProviderType && q.TimeFrame == timeFrame)
                .OrderBy(q => q.Date)
                .Select(q => q.Date)
                .LastOrDefaultAsync();

            var asset = await _dbContext.GetRepository<AssetEntity>()
                .Get(asset => asset.Id == assetId)
                .SingleOrFailAsync();
            
            while (DateWithTimeFrameExtensions.DatesDifferent(prevBatchSmallestDate, lastQuoteDate, timeFrame) > 0)
            {
                var lastBatchQuotes = await GetNextQuotesBatch(quotesProviderType, asset, timeFrame, prevBatchSmallestDate);

                var lastBatchSmallestDate = lastBatchQuotes
                    .OrderBy(q => q.Date)
                    .Select(q => q.Date)
                    .FirstOrDefault();

                if (lastBatchSmallestDate == default || lastBatchSmallestDate >= prevBatchSmallestDate)
                    break;

                var existingQuotesDates = await _dbContext.GetRepository<QuoteEntity>()
                    .Get(q => q.AssetId == assetId
                              && q.QuotesProviderType == quotesProviderType
                              && q.TimeFrame == timeFrame
                              && lastBatchQuotes.Select(qe => qe.Date).Contains(q.Date))
                    .Select(q => q.Date)
                    .ToListAsync();

                _dbContext.GetRepository<QuoteEntity>().InsertRange(lastBatchQuotes.Where(qe => !existingQuotesDates.Contains(qe.Date)).ToList());
                await _dbContext.SaveChangesAsync();

                prevBatchSmallestDate = lastBatchSmallestDate;
            }
        }
    }

    #region private

    private async Task<List<QuoteEntity>> GetNextQuotesBatch(QuotesProviderTypeEnum quotesProviderType, AssetEntity asset, TimeFrameEnum timeFrame, DateTime prevBatchSmallestDate)
    {
        var adapter = _serviceProvider.GetQuotesProviderAdapter(quotesProviderType);

        var batch = await adapter.GetQuotesBatch(asset, timeFrame, prevBatchSmallestDate);
        var quoteEntities = _mapper.Map<List<QuoteEntity>>(batch);
        
        quoteEntities.ForEach(quote =>
        {
            quote.TimeFrame = timeFrame;
            quote.AssetId = asset.Id;
            quote.QuotesProviderType = quotesProviderType;

            quote.Date = quote.Date.CorrectDateByTf(timeFrame);

            quote.Validate();
        });

        return quoteEntities;
    }

    #endregion
}