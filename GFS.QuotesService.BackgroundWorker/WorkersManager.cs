using GFS.BkgWorker.Abstraction;
using GFS.EF.Repository;
using GFS.QuotesService.Api.Common.Enum;
using GFS.QuotesService.BackgroundWorker.Models;
using GFS.QuotesService.BackgroundWorker.TaskContexts;
using GFS.QuotesService.BackgroundWorker.TaskGetters;
using GFS.QuotesService.BackgroundWorker.Workers;
using GFS.QuotesService.BL.Services;

namespace GFS.QuotesService.BackgroundWorker;

public static class WorkersManager
{   
    private static readonly Dictionary<QuotesProviderTypeEnum, ITaskExecutor> Executors = new();

    public static void Init(IServiceProvider serviceProvider)
    {
        TaskExecutor<GetQuotesTaskContext> CreateGetQuotesWorker(QuotesProviderTypeEnum quotesProviderType)
        {
            var dbContext = serviceProvider.GetRequiredService<IDbContext>();
            var taskGetter = new GetQuotesTaskGetter(new GetQuotesTaskGetterModel { QuotesProviderType = quotesProviderType }, dbContext);
            var logger = serviceProvider.GetRequiredService<ILogger<GetQuotesWorker>>();
            var getDataFromProviderService = serviceProvider.GetRequiredService<IQuotesProviderService>();
            
            return new GetQuotesWorker(1, 50, logger, getDataFromProviderService, taskGetter);
        }

        var initialAssetsWorker = new InitialAssetsWorker(serviceProvider.GetRequiredService<ILogger<InitialAssetsWorker>>(), serviceProvider.GetRequiredService<IQuotesProviderService>());
        var tinkoffGetQuotesWorker = CreateGetQuotesWorker(QuotesProviderTypeEnum.Tinkoff);
        // var bcsExpressGetQuotesWorker = CreateGetQuotesWorker(QuotesProviderTypeEnum.BcsExpress);
        // var finamGetQuotesWorker =  CreateGetQuotesWorker(QuotesProviderTypeEnum.Finam);
        // var investingComGetQuotesWorker=  CreateGetQuotesWorker(QuotesProviderTypeEnum.InvestingCom);
        
        Executors.Add(QuotesProviderTypeEnum.Tinkoff, tinkoffGetQuotesWorker);
        // Executors.Add(QuotesProviderTypeEnum.BcsExpress, bcsExpressGetQuotesWorker);
        // Executors.Add(QuotesProviderTypeEnum.Finam, finamGetQuotesWorker);
        // Executors.Add(QuotesProviderTypeEnum.InvestingCom, investingComGetQuotesWorker);
    } 

    public static bool TryGetTaskExecutor(QuotesProviderTypeEnum quotesProviderType, out ITaskExecutor? executor)
        => Executors.TryGetValue(quotesProviderType, out executor);
}