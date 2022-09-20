using System.Transactions;
using AutoMapper;
using GFS.EF.Repository;
using GFS.QuotesService.BL.QuotesProviderAdapters;
using GFS.QuotesService.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace GFS.QuotesService.BL.Services;

public interface IGetDataFromProviderService
{
    Task InitialFromMainAdapter(bool anyway = false);
}

internal class GetDataFromProviderFromProviderService : IGetDataFromProviderService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Mapper _mapper;
    private readonly IDbContext _dbContext;

    public GetDataFromProviderFromProviderService(
        IServiceProvider serviceProvider,
        Mapper mapper,
        IDbContext dbContext)
    {
        _serviceProvider = serviceProvider;
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task InitialFromMainAdapter(bool anyway)
    {
        using var transaction = new TransactionScope();

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
            asset.Id = Guid.NewGuid();
            asset.AssetInfo = _mapper.Map<AssetInfoEntity>(im);
            asset.AssetInfo.AssetId = asset.Id;
            return asset;
        }).ToList();

        assetRepository.InsertRange(assets);
        await _dbContext.SaveChangesAsync();

        transaction.Complete();
    }
}