using System.ComponentModel.DataAnnotations;
using GFS.Common.Extensions;
using GFS.Common.Interfaces;
using GFS.Portfolio.Common.Models;

#pragma warning disable CS8618
namespace GFS.TradingStrategyTester.Common.Models;

public class CommonSettings : IValidatedModel
{
    [Required]
    public TradingStrategyTesterSettings TradingStrategyTesterSettings { get; init; }
    
    [Required]
    public PortfolioSettings PortfolioSettings { get; init; }







    public void ValidateModel()
    {
        this.Validate();
        
        TradingStrategyTesterSettings.ValidateModel();
        PortfolioSettings.ValidateModel();
    }
}