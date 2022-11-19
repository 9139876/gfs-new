using GFS.BkgWorker.Task;
using GFS.QuotesService.Api.Common.Enum;

namespace GFS.QuotesService.BackgroundWorker.TaskContexts;

public class GetInitialDataTaskContext : TaskContext
{
    public GetInitialDataTaskContext(QuotesProviderTypeEnum quotesProviderType)
    {
        QuotesProviderType = quotesProviderType;
    }
    
    public QuotesProviderTypeEnum QuotesProviderType { get; }
    
    public override Task<bool> DoWork(IServiceProvider serviceProvider)
    {
        throw new NotImplementedException();
    }

    protected override string SerializeImpl()
    {
        throw new NotImplementedException();
    }
}