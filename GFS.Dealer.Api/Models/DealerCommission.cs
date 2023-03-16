using GFS.Common.Attributes.Validation;
using GFS.FakeDealer.Api.Enums;

namespace GFS.FakeDealer.Api.Models;

public class DealerCommission
{
    public DealerCommissionType CommissionType { get; init; }
    
    [PositiveNumberOrZero]
    public decimal Value { get; init; }
}