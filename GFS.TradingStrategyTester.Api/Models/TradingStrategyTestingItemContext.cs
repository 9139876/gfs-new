using System.ComponentModel.DataAnnotations;
using GFS.BackgroundWorker.Models;
using GFS.TradingStrategyTester.Common.Models;
#pragma warning disable CS8618

namespace GFS.TradingStrategyTester.Api.Models;

public class TradingStrategyTestingItemContext : IBkgWorkerTaskContext
{
    [Required]
    public CommonSettings TestingItemSettings { get; init; }
    
    
    
    
    
    public int GetPriority() => 1;
}