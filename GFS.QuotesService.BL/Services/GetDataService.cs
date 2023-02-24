using GFS.EF.Repository;
using GFS.QuotesService.Api.Models;
using GFS.QuotesService.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace GFS.QuotesService.BL.Services;

public interface IGetDataService
{
    Task<List<AssetEntity>> GetAssetsInfo(AssetsFilter request);

    Task<QuotesInfoDto> GetAssetQuotesInfo(Guid assetId);
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

    public Task<QuotesInfoDto> GetAssetQuotesInfo(Guid assetId)
    {
        throw new NotImplementedException();
    }
}