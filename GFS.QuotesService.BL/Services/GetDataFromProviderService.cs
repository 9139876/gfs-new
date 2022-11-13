using AutoMapper;
using GFS.Common.Helpers;
using GFS.EF.Repository;
using GFS.GrailCommon.Enums;
using GFS.GrailCommon.Models;
using GFS.QuotesService.Api.Enum;
using GFS.QuotesService.BL.Extensions;
using GFS.QuotesService.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace GFS.QuotesService.BL.Services;

public interface IGetDataFromProviderService
{
    Task InitialFromMainAdapter(bool anyway = false);
    Task GetAndSaveNextQuotesBatch(QuotesProviderTypeEnum quotesProviderType, Guid assetId, TimeFrameEnum timeFrame);
}

internal class GetDataFromProviderService : IGetDataFromProviderService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IMapper _mapper;
    private readonly IDbContext _dbContext;

    public GetDataFromProviderService(
        IServiceProvider serviceProvider,
        IMapper mapper,
        IDbContext dbContext)
    {
        _serviceProvider = serviceProvider;
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task InitialFromMainAdapter(bool anyway)
    {
        using var transaction = SystemTransaction.Default();

        var assetRepository = _dbContext.GetRepository<AssetEntity>();

        if (await assetRepository.Exist())
        {
            if (!anyway)
                return;

            assetRepository.DeleteRange(await assetRepository.Get().ToListAsync());
            await _dbContext.SaveChangesAsync();
        }

        var mainAdapter = _serviceProvider.GetMainQuotesProviderAdapter();
        var initialModels = await mainAdapter.GetInitialData();

        var assets = initialModels.Select(im =>
        {
            var asset = _mapper.Map<AssetEntity>(im);
            asset.AssetInfo = _mapper.Map<AssetInfoEntity>(im);
            asset.AssetInfo.AssetId = asset.Id;
            return asset;
        }).ToList();

        assetRepository.InsertRange(assets);
        await _dbContext.SaveChangesAsync();

        transaction.Complete();
    }

    public async Task GetAndSaveNextQuotesBatch(QuotesProviderTypeEnum quotesProviderType, Guid assetId, TimeFrameEnum timeFrame)
    {
        var adapter = _serviceProvider.GetQuotesProviderAdapter(quotesProviderType);

        if (!adapter.IsNativeSupportedTimeframe(timeFrame))
            return;

        var quotesProviderAsset = await _dbContext.GetRepository<QuotesProviderAssetEntity>()
            .Get(qpa => qpa.AssetId == assetId && qpa.QuotesProviderType == quotesProviderType)
            .Include(qpa => qpa.Asset)
            .SingleOrDefaultAsync();

        if (quotesProviderAsset == null)
        {
            quotesProviderAsset = new()
            {
                AssetId = assetId,
                QuotesProviderType = quotesProviderType
            };

            _dbContext.GetRepository<QuotesProviderAssetEntity>().Insert(quotesProviderAsset);
            await _dbContext.SaveChangesAsync();

            quotesProviderAsset.Asset = await _dbContext.GetRepository<AssetEntity>().SingleOrFailById(assetId);
        }

        var lastQuote = await _dbContext.GetRepository<QuoteEntity>()
            .Get(q => q.QuotesProviderAssetId == quotesProviderAsset.Id && q.TimeFrame == timeFrame)
            .OrderBy(q => q.Date)
            .LastOrDefaultAsync();

        //ToDo Сделать нормально!!!
        //Пока не нужны суперсвежие котировки нечего бесконечно долбить провайдера
        if((DateTime.Now - lastQuote.Date).Days <= 1)
            return;
        
        var batch = await adapter.GetQuotesBatch(quotesProviderAsset.Asset!, timeFrame, lastQuote != null ? _mapper.Map<QuoteModel>(lastQuote) : null);
        var quoteEntities = _mapper.Map<List<QuoteEntity>>(batch);
        quoteEntities.ForEach(quote =>
        {
            quote.TimeFrame = timeFrame;
            quote.QuotesProviderAssetId = quotesProviderAsset.Id;
        });

        _dbContext.GetRepository<QuoteEntity>().InsertRange(quoteEntities);
        await _dbContext.SaveChangesAsync();
    }
}