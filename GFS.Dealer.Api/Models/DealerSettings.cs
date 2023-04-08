using GFS.FakeDealer.Api.Enums;
using GFS.GrailCommon.Enums;
using GFS.QuotesService.Common.Enum;

#pragma warning disable CS8618

namespace GFS.FakeDealer.Api.Models;

public class DealerSettings
{
    public QuotesProviderTypeEnum QuotesProviderType { get; init; }

    public DealPriceCalcBehaviorEnum DealPriceCalcBehavior { get; init; }

    public TimeFrameEnum TimeFrame { get; init; }
    
    public DealerCommission DealerCommission { get; init; }
}