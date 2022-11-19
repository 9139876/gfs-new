using GFS.BkgWorker.Worker;
using GFS.QuotesService.Api.Common.Enum;

namespace GFS.QuotesService.BackgroundWorker;

public static class WorkersManager
{   
    private static readonly Dictionary<QuotesProviderTypeEnum, IWorker> Workers = new();

    public static void Init(IServiceProvider serviceProvider)
    {
        IWorker CreateGetQuotesWorker()
        {
            var worker = serviceProvider.GetRequiredService<IWorker>();
            worker.Start(1);
            return worker;
        }
        
        Workers.Add(QuotesProviderTypeEnum.Tinkoff, CreateGetQuotesWorker());
        Workers.Add(QuotesProviderTypeEnum.BcsExpress, CreateGetQuotesWorker());
        Workers.Add(QuotesProviderTypeEnum.Finam, CreateGetQuotesWorker());
        Workers.Add(QuotesProviderTypeEnum.InvestingCom, CreateGetQuotesWorker());
    } 

    public static bool TryGetWorker(QuotesProviderTypeEnum quotesProviderType, out IWorker? worker)
        => Workers.TryGetValue(quotesProviderType, out worker);
}