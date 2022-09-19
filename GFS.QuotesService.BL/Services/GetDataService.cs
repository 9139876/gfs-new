using AutoMapper;
using GFS.EF.Repository;
using GFS.QuotesService.BL.QuotesProviderAdapters;
using GFS.QuotesService.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace GFS.QuotesService.BL.Services;

public interface IGetDataService
{
    Task InitialFromMainAdapter(bool anyway = false);
}

internal class GetDataService : IGetDataService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Mapper _mapper;
    private readonly IDbContext _dbContext;

    public GetDataService(
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
        //add transaction!!!
        
        var marketRepository = _dbContext.GetRepository<MarketEntity>();

        if (await marketRepository.Exist())
        {
            if(!anyway)
                return;
            
            marketRepository.DeleteRange(await marketRepository.Get().ToListAsync());
            await _dbContext.SaveChangesAsync();
        }
        
        if(!anyway && await marketRepository.Exist())
            return;
        
        var mainAdapter = _serviceProvider.GetMainQuotesProviderAdapter();
        
        //получить супермодель
        
        //последовательно маппить в сущности
        
        //записать все в базу

        marketRepository.Insert(null);
        await _dbContext.SaveChangesAsync();
    }
}