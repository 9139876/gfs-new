using GFS.EF.Repository;
using GFS.GrailCommon.Enums;
using GFS.QuotesService.Api.Models;
using GFS.QuotesService.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace GFS.QuotesService.BL.Services;

public interface IGetDataService
{
    Task<List<AssetEntity>> GetAssetsInfo(AssetsFilter request);

    Task<AssetQuotesInfoDto> GetAssetQuotesInfo(Guid assetId);
}

public class GetDataService : IGetDataService
{
    private readonly IDbContext _dbContext;

    public GetDataService(IDbContext dbContext)
    {
        _dbContext = dbContext;
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

    #region private static

    private static async Task<List<AssetTimeFrameQuotesInfoDto>> GetAssetTimeFrameQuotesInfos(IQueryable<QuoteEntity> baseQuery, IEnumerable<TimeFrameEnum> timeFrames)
    {
        List<AssetTimeFrameQuotesInfoDto> result = new();

        foreach (var tf in timeFrames)
        {
            var tfBaseQuery = baseQuery.Where(quote => quote.TimeFrame == tf);

            result.Add(new AssetTimeFrameQuotesInfoDto
            {
                TimeFrame = tf,
                Count = await tfBaseQuery.CountAsync(),
                FirstDate = await tfBaseQuery.Select(quote => quote.Date).MinAsync(),
                LastDate = await tfBaseQuery.Select(quote => quote.Date).MaxAsync()
            });
        }

        return result;
    }

    #endregion
}