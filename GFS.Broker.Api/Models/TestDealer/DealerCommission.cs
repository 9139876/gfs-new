using GFS.Broker.Api.Enums.TestDealer;
using GFS.Common.Attributes.Validation;

namespace GFS.Broker.Api.Models.TestDealer;

public class DealerCommission
{
    public DealerCommissionType CommissionType { get; init; }
    
    [PositiveNumberOrZero]
    public decimal Value { get; init; }
}