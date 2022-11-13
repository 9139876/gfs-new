using GFS.BkgWorker.Abstraction;
using GFS.EF.Repository;
using GFS.QuotesService.Api.Enum;
using GFS.QuotesService.BackgroundWorker.Models;
using GFS.QuotesService.BackgroundWorker.TaskContexts;
using GFS.QuotesService.BackgroundWorker.TaskGetters;
using GFS.QuotesService.BackgroundWorker.Workers;
using GFS.QuotesService.BL.Services;

namespace GFS.QuotesService.BackgroundWorker;

public static class WorkersManager
{
    public const string InitialAssetsWorker = "InitialAssetsWorker";
    public const string TinkoffGetQuotesWorker = "TinkoffGetQuotesWorker";
    public const string BcsExpressGetQuotesWorker = "BcsExpressGetQuotesWorker";
    public const string FinamGetQuotesWorker = "FinamGetQuotesWorker";
    public const string InvestingComGetQuotesWorker = "InvestingComGetQuotesWorker";
    
    
    private static readonly Dictionary<string, ITaskExecutor> Executors = new();

    public static void Init(IServiceProvider serviceProvider)
    {
        TaskExecutor<GetQuotesTaskContext> CreateGetQuotesWorker(QuotesProviderTypeEnum quotesProviderType)
        {
            var dbContext = serviceProvider.GetRequiredService<IDbContext>();
            var taskGetter = new GetQuotesTaskGetter(new GetQuotesTaskGetterModel { QuotesProviderType = quotesProviderType }, dbContext);
            var logger = serviceProvider.GetRequiredService<ILogger<GetQuotesWorker>>();
            var getDataFromProviderService = serviceProvider.GetRequiredService<IGetDataFromProviderService>();
            
            return new GetQuotesWorker(1, 50, logger, getDataFromProviderService, taskGetter);
        }

        var initialAssetsWorker = new InitialAssetsWorker(serviceProvider.GetRequiredService<ILogger<InitialAssetsWorker>>(), serviceProvider.GetRequiredService<IGetDataFromProviderService>());
        var tinkoffGetQuotesWorker = CreateGetQuotesWorker(QuotesProviderTypeEnum.Tinkoff);
        // var bcsExpressGetQuotesWorker = CreateGetQuotesWorker(QuotesProviderTypeEnum.BcsExpress);
        // var finamGetQuotesWorker =  CreateGetQuotesWorker(QuotesProviderTypeEnum.Finam);
        // var investingComGetQuotesWorker=  CreateGetQuotesWorker(QuotesProviderTypeEnum.InvestingCom);
        
        Executors.Add(InitialAssetsWorker, initialAssetsWorker);
        Executors.Add(TinkoffGetQuotesWorker, tinkoffGetQuotesWorker);
        // Executors.Add(BcsExpressGetQuotesWorker, bcsExpressGetQuotesWorker);
        // Executors.Add(FinamGetQuotesWorker, finamGetQuotesWorker);
        // Executors.Add(InvestingComGetQuotesWorker, investingComGetQuotesWorker);
    } 

    public static bool TryGetTaskExecutor(string name, out ITaskExecutor? executor)
        => Executors.TryGetValue(name, out executor);
}