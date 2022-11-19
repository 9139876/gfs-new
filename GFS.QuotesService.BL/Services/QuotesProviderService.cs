using AutoMapper;
using GFS.Common.Helpers;
using GFS.EF.Extensions;
using GFS.EF.Repository;
using GFS.GrailCommon.Enums;
using GFS.GrailCommon.Models;
using GFS.QuotesService.Api.Common.Enum;
using GFS.QuotesService.BL.Extensions;
using GFS.QuotesService.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace GFS.QuotesService.BL.Services;

public interface IQuotesProviderService
{
    Task InitialAssets(QuotesProviderTypeEnum quotesProviderType);
    Task<DateTime> GetAndSaveNextQuotesBatch(QuotesProviderTypeEnum quotesProviderType, Guid assetId, TimeFrameEnum timeFrame);
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

        var assetProviderCompatibilities = assets.Select(asset => new AssetProviderCompatibilityEntity
        {
            AssetId = asset.Id,
            //ToDo Убрать статику!!!
            QuotesProviderType = QuotesProviderTypeEnum.Tinkoff,
            IsCompatibility = true
        });

        _dbContext.GetRepository<AssetProviderCompatibilityEntity>().InsertRange(assetProviderCompatibilities);

        await _dbContext.SaveChangesAsync();
        transaction.Complete();
    }

    public async Task<DateTime> GetAndSaveNextQuotesBatch(QuotesProviderTypeEnum quotesProviderType, Guid assetId, TimeFrameEnum timeFrame)
    {
        var adapter = _serviceProvider.GetQuotesProviderAdapter(quotesProviderType);

        if (!adapter.IsNativeSupportedTimeframe(timeFrame))
            return;

        var asset = await _dbContext.GetRepository<AssetEntity>()
            .Get(asset => asset.Id == assetId)
            .SingleOrFailAsync();

        var lastQuote = await _dbContext.GetRepository<QuoteEntity>()
            .Get(q => q.AssetId == assetId && q.QuotesProviderType == quotesProviderType && q.TimeFrame == timeFrame)
            .OrderBy(q => q.Date)
            .LastOrDefaultAsync();

        //ToDo Сделать нормально!!!
        //Пока не нужны суперсвежие котировки нечего бесконечно долбить провайдера
        if (lastQuote != null && (DateTime.Now - lastQuote.Date).Days <= 1)
            return;

        var batch = await adapter.GetQuotesBatch(asset, timeFrame, lastQuote != null ? _mapper.Map<QuoteModel>(lastQuote) : null);
        var quoteEntities = _mapper.Map<List<QuoteEntity>>(batch);
        quoteEntities.ForEach(quote =>
        {
            quote.TimeFrame = timeFrame;
            quote.AssetId = assetId;
            quote.QuotesProviderType = quotesProviderType;
        });

        _dbContext.GetRepository<QuoteEntity>().InsertRange(quoteEntities);
        await _dbContext.SaveChangesAsync();
    }
}