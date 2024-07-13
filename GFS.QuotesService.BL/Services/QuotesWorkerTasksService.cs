using GFS.EF.Extensions;
using GFS.EF.Repository;
using GFS.GrailCommon.Extensions;
using GFS.QuotesService.Api.Models;
using GFS.QuotesService.BL.Models;
using GFS.QuotesService.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace GFS.QuotesService.BL.Services;

public interface IQuotesWorkerTasksService
{
    Task AddGetQuotesTask(AddGetQuotesTaskRequest request);
}

internal class QuotesWorkerTasksService : IQuotesWorkerTasksService
{
    private readonly IQuotesProviderService _quotesProviderService;
    private readonly IDbContext _dbContext;

    public QuotesWorkerTasksService(
        IQuotesProviderService quotesProviderService,
        IDbContext dbContext)
    {
        _quotesProviderService = quotesProviderService;
        _dbContext = dbContext;
    }


    public async Task AddGetQuotesTask(AddGetQuotesTaskRequest request)
    {
        var assetProvider = await _dbContext.GetRepository<AssetProviderCompatibilityEntity>()
            .Get(ap => ap.AssetId == request.AssetId && ap.TimeFrame == request.TimeFrame && ap.QuotesProviderType == request.QuotesProviderType)
            .SingleOrFailAsync();

        var existingUpdateQuotesTaskEntity = await _dbContext.GetRepository<UpdateQuotesTaskEntity>()
            .Get(task => task.AssetByProviderId == assetProvider.Id)
            .SingleOrDefaultAsync();

        if (existingUpdateQuotesTaskEntity != null)
        {
            existingUpdateQuotesTaskEntity.IsActive = true;
            await _dbContext.SaveChangesAsync();
            return;
        }

        var getFirstQuoteRequest = new GetFirstQuoteRequestModel
        {
            Asset = await _dbContext.GetRepository<AssetEntity>().SingleOrFailByIdAsync(request.AssetId),
            QuotesProviderType = request.QuotesProviderType,
            TimeFrame = request.TimeFrame
        };

        var firstQuoteDate = await _quotesProviderService.GetFirstQuoteDate(getFirstQuoteRequest);

        var newUpdateQuotesTaskEntity = new UpdateQuotesTaskEntity
        {
            AssetByProviderId = assetProvider.Id,
            LastQuoteDate = firstQuoteDate.CorrectDateByTf(request.TimeFrame),
            IsActive = true
        };

        _dbContext.GetRepository<UpdateQuotesTaskEntity>().Insert(newUpdateQuotesTaskEntity);
        await _dbContext.SaveChangesAsync();
    }
}