using GFS.BkgWorker.Task;
using GFS.QuotesService.Api.Common.Enum;
using GFS.QuotesService.BL.Services;

namespace GFS.QuotesService.BackgroundWorker.TaskContexts;

public class GetInitialDataTaskContext : TaskContext
{
    public GetInitialDataTaskContext(QuotesProviderTypeEnum quotesProviderType)
    {
        QuotesProviderType = quotesProviderType;
    }
    
    public QuotesProviderTypeEnum QuotesProviderType { get; }
    
    public async override Task<bool> DoWork(IServiceProvider serviceProvider)
    {
        var quotesProviderService = serviceProvider.GetRequiredService<IQuotesProviderService>();
        await quotesProviderService.InitialAssets(QuotesProviderType);
        return false;
    }

    protected override string SerializeImpl()
        => QuotesProviderType.ToString();
}