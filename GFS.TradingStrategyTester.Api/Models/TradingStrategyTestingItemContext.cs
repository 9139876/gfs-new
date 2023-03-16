using GFS.BackgroundWorker.Models;

namespace GFS.TradingStrategyTester.Api.Models;

public class TradingStrategyTestingItemContext : IBkgWorkerTaskContext
{
    
    
    
    
    
    
    public int GetPriority() => 1;
}