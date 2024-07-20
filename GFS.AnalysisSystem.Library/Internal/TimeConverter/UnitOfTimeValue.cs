using GFS.Common.Extensions;

namespace GFS.AnalysisSystem.Library.Internal.TimeConverter;

internal class UnitOfTimeValue
{
    public UnitOfTimeValue(UnitOfTime unitOfTime, decimal value)
    {
        UnitOfTime = unitOfTime;
        Value = value;
    }

    public UnitOfTime UnitOfTime { get; init; }
    public decimal Value { get; init; }

    public string Description => $"{Common.Attributes.Description.GetDescription(UnitOfTime)}: {Value.ToHumanReadableNumber()}";
}