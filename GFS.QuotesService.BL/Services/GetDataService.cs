using AutoMapper;
using GFS.EF.Repository;
using GFS.GrailCommon.Enums;
using GFS.GrailCommon.Models;
using GFS.QuotesService.Api.Models;
using GFS.QuotesService.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace GFS.QuotesService.BL.Services;

public interface IGetDataService
{
    Task<List<AssetEntity>> GetAssetsInfo(AssetsFilter request);

    Task<AssetQuotesInfoDto> GetAssetQuotesInfo(Guid assetId);

    Task<GetQuotesResponse> GetQuotes(GetQuotesRequest request);
}

public class GetDataService : IGetDataService
{
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetDataService(
        IDbContext dbContext,
        IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<AssetEntity>> GetAssetsInfo(AssetsFilter request)
    {
        var assetRepository = _dbContext.GetRepository<AssetEntity>();

        var selectors = new List<Func<IQueryable<AssetEntity>, IQueryable<AssetEntity>>>
        {
            query => query.Include(asset => asset.AssetInfo).AsNoTracking()
        };

        if (request.OnlyHasQuotes)
            selectors.Add(query => query.Where(asset => asset.Quotes.Any()));

        if (request.AssetId.HasValue)
            selectors.Add(query => query.Where(asset => asset.Id == request.AssetId));

        if (!string.IsNullOrWhiteSpace(request.FIGI))
            selectors.Add(query => query.Where(asset => asset.FIGI == request.FIGI));

        if (!string.IsNullOrWhiteSpace(request.NameFilter))
            selectors.Add(query => query.Where(asset => asset.Name.ToUpper().Contains(request.NameFilter.ToUpper())));

        return await assetRepository.GetBySelectors(selectors).ToListAsync();
    }

    public async Task<AssetQuotesInfoDto> GetAssetQuotesInfo(Guid assetId)
    {
        var baseQuery = _dbContext.GetRepository<QuoteEntity>().Get(quote => quote.AssetId == assetId).AsNoTracking();

        var existingTimeFrames = await baseQuery.Select(q => q.TimeFrame).Distinct().ToListAsync();

        if (!existingTimeFrames.Any())
            return new AssetQuotesInfoDto();

        return new AssetQuotesInfoDto
        {
            MinPrice = await baseQuery.Select(quote => quote.Low).MinAsync(),
            MaxPrice = await baseQuery.Select(quote => quote.High).MaxAsync(),
            AssetTimeFrameQuotesInfos = await GetAssetTimeFrameQuotesInfos(baseQuery, existingTimeFrames)
        };
    }

    public async Task<GetQuotesResponse> GetQuotes(GetQuotesRequest request)
    {
        var query = _dbContext.GetRepository<QuoteEntity>()
            .Get(quote => quote.AssetId == request.AssetId
                          && quote.TimeFrame == request.TimeFrame
                          && quote.QuotesProviderType == request.QuotesProviderType)
            .OrderBy(q => q.Date)
            .AsNoTracking();

        if (request.StartDate.HasValue)
            query = query.Where(quote => quote.Date >= request.StartDate);

        if (request.EndDate.HasValue)
            query = query.Where(quote => quote.Date <= request.EndDate);


        var quotes = await query.ToListAsync();

        return new GetQuotesResponse
        {
            Quotes = _mapper.Map<List<QuoteModel>>(quotes)
        };
    }

    #region private static

    private static async Task<List<AssetTimeFrameQuotesInfoDto>> GetAssetTimeFrameQuotesInfos(IQueryable<QuoteEntity> baseQuery, IEnumerable<TimeFrameEnum> timeFrames)
    {
        List<AssetTimeFrameQuotesInfoDto> result = new();

        foreach (var tf in timeFrames)
        {
            var tfBaseQuery = baseQuery.Where(quote => quote.TimeFrame == tf);

            var minPriceValue = await tfBaseQuery.OrderBy(quote => quote.Low).Select(quote => new { quote.Low, quote.Date }).FirstAsync();
            var maxPriceValue = await tfBaseQuery.OrderBy(quote => quote.High).Select(quote => new { quote.High, quote.Date }).LastAsync();

            result.Add(new AssetTimeFrameQuotesInfoDto
            {
                TimeFrame = tf,
                Count = await tfBaseQuery.CountAsync(),
                FirstDate = await tfBaseQuery.Select(quote => quote.Date).MinAsync(),
                LastDate = await tfBaseQuery.Select(quote => quote.Date).MaxAsync(),
                MinPrice = minPriceValue.Low,
                MinPriceDate = minPriceValue.Date,
                MaxPrice = maxPriceValue.High,
                MaxPriceDate = maxPriceValue.Date
            });
        }

        return result;
    }

    #endregion
}