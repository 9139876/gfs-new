using GFS.FakeDealer.Api.Enums;
using GFS.GrailCommon.Enums;
using GFS.QuotesService.Api.Common.Enum;

namespace GFS.FakeDealer.Api.Models;

public class DealerSettings
{
    public QuotesProviderTypeEnum QuotesProviderType { get; init; }

    public DealPriceCalcBehaviorEnum DealPriceCalcBehavior { get; init; }

    public TimeFrameEnum TimeFrame { get; set; }
}