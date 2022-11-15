using GFS.BkgWorker.Abstraction;
using GFS.EF.Repository;
using GFS.GrailCommon.Enums;
using GFS.QuotesService.BackgroundWorker.Models;
using GFS.QuotesService.BackgroundWorker.TaskContexts;
using GFS.QuotesService.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace GFS.QuotesService.BackgroundWorker.TaskGetters;

public class GetQuotesTaskGetter : ITaskGetter<GetQuotesTaskContext>
{
    private readonly IDbContext _dbContext;

    public GetQuotesTaskGetter(
        ITaskGetterModel model,
        IDbContext dbContext)
    {
        Model = model;
        _dbContext = dbContext;
    }

    public ITaskGetterModel Model { get; }

    public async Task<IEnumerable<GetQuotesTaskContext>> GetTasks()
    {
        if (Model is not GetQuotesTaskGetterModel model)
            throw new InvalidCastException($"{nameof(Model)} is not {nameof(GetQuotesTaskGetterModel)}");

        var dbTasks = await _dbContext.GetRepository<BackgroundWorkerTaskEntity>()
            .Get(bwt => bwt.QuotesProviderType == model.QuotesProviderType)
            .Include(bwt => bwt.Asset)
            .ToListAsync();

        return from dbTask in dbTasks from timeFrame in Enum.GetValues<TimeFrameEnum>() select new GetQuotesTaskContext(dbTask.AssetId ?? Guid.Empty, model.QuotesProviderType, timeFrame);
    }
}