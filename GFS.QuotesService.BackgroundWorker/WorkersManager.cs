using GFS.BkgWorker.Abstraction;
using GFS.EF.Repository;
using GFS.QuotesService.Api.Enum;
using GFS.QuotesService.BackgroundWorker.Models;
using GFS.QuotesService.BackgroundWorker.TaskGetters;
using GFS.QuotesService.BackgroundWorker.Workers;
using GFS.QuotesService.BL.Services;

namespace GFS.QuotesService.BackgroundWorker;

public static class WorkersManager
{
    private static readonly Dictionary<string, ITaskExecutor> Executors = new();

    public static void Init(IServiceProvider serviceProvider)
    {
        var initialAssetsWorker = new InitialAssetsWorker(serviceProvider.GetRequiredService<ILogger>(), serviceProvider.GetRequiredService<IGetDataFromProviderService>());

        var dbContext = serviceProvider.GetRequiredService<IDbContext>();

        var tinkoffGetQuotesTaskGetter = new GetQuotesTaskGetter(new GetQuotesTaskGetterModel { QuotesProviderType = QuotesProviderTypeEnum.Tinkoff }, dbContext);
        var tinkoffGetQuotesWorker = new TinkoffGetQuotesWorker(1, 50, serviceProvider.GetRequiredService<ILogger>(), tinkoffGetQuotesTaskGetter);

        var bcsExpressGetQuotesTaskGetter = new GetQuotesTaskGetter(new GetQuotesTaskGetterModel { QuotesProviderType = QuotesProviderTypeEnum.BcsExpress }, dbContext);
        var bcsExpressGetQuotesWorker = new BcsExpressGetQuotesWorker(1, 50, serviceProvider.GetRequiredService<ILogger>(), bcsExpressGetQuotesTaskGetter);

        var finamGetQuotesTaskGetter = new GetQuotesTaskGetter(new GetQuotesTaskGetterModel { QuotesProviderType = QuotesProviderTypeEnum.Finam }, dbContext);
        var finamGetQuotesWorker = new FinamGetQuotesWorker(1, 50, serviceProvider.GetRequiredService<ILogger>(), finamGetQuotesTaskGetter);

        var investingComGetQuotesTaskGetter = new GetQuotesTaskGetter(new GetQuotesTaskGetterModel { QuotesProviderType = QuotesProviderTypeEnum.InvestingCom }, dbContext);
        var investingComGetQuotesWorker = new InvestingComGetQuotesWorker(1, 50, serviceProvider.GetRequiredService<ILogger>(), investingComGetQuotesTaskGetter);

        Executors.Add(typeof(InitialAssetsWorker).FullName!, initialAssetsWorker);
        Executors.Add(typeof(TinkoffGetQuotesWorker).FullName!, tinkoffGetQuotesWorker);
        Executors.Add(typeof(BcsExpressGetQuotesWorker).FullName!, bcsExpressGetQuotesWorker);
        Executors.Add(typeof(FinamGetQuotesWorker).FullName!, finamGetQuotesWorker);
        Executors.Add(typeof(InvestingComGetQuotesWorker).FullName!, investingComGetQuotesWorker);
    }

    public static bool TryGetTaskExecutor(string fullName, out ITaskExecutor? executor)
        => Executors.TryGetValue(fullName, out executor);
}