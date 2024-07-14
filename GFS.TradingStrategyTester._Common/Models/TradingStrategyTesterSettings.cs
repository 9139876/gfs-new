using System.ComponentModel.DataAnnotations;
using GFS.Common.Extensions;
using GFS.Common.Interfaces;
using GFS.GrailCommon.Enums;
using GFS.QuotesService.Api.Enum;

#pragma warning disable CS8618

namespace GFS.TradingStrategyTester.Common.Models;

public class TradingStrategyTesterSettings : IValidatedModel
{
    [Required]
    public QuotesProviderTypeEnum QuotesProviderType { get; init; }
    
    [Required]
    public TimeFrameEnum TimeFrame { get; init; }

    [Required]
    public List<Guid> AssetsIds { get; init; }

    public void ValidateModel()
    {
        this.Validate();
    }
}