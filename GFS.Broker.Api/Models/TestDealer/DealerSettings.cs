using GFS.Broker.Api.Enums.TestDealer;
using GFS.GrailCommon.Enums;
using GFS.QuotesService.Api.Enum;

#pragma warning disable CS8618

namespace GFS.Broker.Api.Models.TestDealer;

public class DealerSettings
{
    public QuotesProviderTypeEnum QuotesProviderType { get; init; }

    public DealPriceCalcBehaviorEnum DealPriceCalcBehavior { get; init; }

    public TimeFrameEnum TimeFrame { get; init; }
    
    public DealerCommission DealerCommission { get; init; }
}