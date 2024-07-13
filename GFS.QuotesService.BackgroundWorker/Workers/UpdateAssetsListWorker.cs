using GFS.BackgroundWorker.Workers;
using GFS.QuotesService.BL.Services;
using GFS.QuotesService.Common.Enum;
using GFS.QuotesService.DAL;
using GFS.QuotesService.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GFS.QuotesService.BackgroundWorker.Workers;

internal class UpdateAssetsListWorker : SimpleWorker<UpdateAssetsListTaskData>
{
    private readonly IOptions<WorkersSettings> _workersSettings;

    public UpdateAssetsListWorker(IServiceProvider serviceProvider) : base(serviceProvider, serviceProvider.GetRequiredService<ILogger<UpdateAssetsListWorker>>())
    {
        _workersSettings = serviceProvider.GetRequiredService<IOptions<WorkersSettings>>();
    }

    protected override TimeSpan SleepTime => TimeSpan.FromSeconds(_workersSettings.Value.UpdateAssetsListSleepInSeconds);

    protected override Task<List<UpdateAssetsListTaskData>> GetTasksData(IServiceProvider serviceProvider)
    {
        return Task.FromResult(new List<UpdateAssetsListTaskData> { new() });
    }

    protected override async Task<TaskExecutingResult<UpdateAssetsListTaskData>> DoTaskInternal(IServiceProvider serviceProvider, UpdateAssetsListTaskData taskDataItem)
    {
        var dbContext = serviceProvider.GetRequiredService<QuotesServiceDbContext>();
        var quotesProviderService = serviceProvider.GetRequiredService<IQuotesProviderService>();
        
        var receivedAssets = new List<AssetEntity>();

        foreach (var quotesProviderType in ActiveQuotesProviders)
        {
            try
            {
                var assets = await quotesProviderService.GetAssetsList(quotesProviderType);
                receivedAssets.AddRange(assets);
                Logger.LogInformation("От провайдера {provider} получено {count} активов", quotesProviderType, assets.Count);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Ошибка получения списка активов от провайдера {provider} - {error}", quotesProviderType, e.Message);
            }
        }

        var result = new TaskExecutingResult<UpdateAssetsListTaskData>(string.Empty, null);

        if (!receivedAssets.Any())
            return result;

        var existingAssets = await dbContext.GetRepository<AssetEntity>().Get().ToListAsync();
        var newAssets = receivedAssets.Except(existingAssets, new AssetEntityComparerByFifi()).ToList();

        Logger.LogInformation("Всего получено новых активов - {count}", newAssets.Count);

        if (!newAssets.Any())
            return result;

        dbContext.GetRepository<AssetEntity>().InsertRange(newAssets);
        await dbContext.SaveChangesAsync();

        Logger.LogInformation("Новые активы успешно сохранены в БД");

        return result;
    }

    private static IEnumerable<QuotesProviderTypeEnum> ActiveQuotesProviders => new[] { QuotesProviderTypeEnum.Tinkoff };
}

internal class UpdateAssetsListTaskData : ILoggingSerializable
{
    public string Serialize()
    {
        return string.Empty;
    }
}