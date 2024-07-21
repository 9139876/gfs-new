using GFS.Common.Extensions;

namespace GFS.AnalysisSystem.Library.Internal.TimeConverter;

internal class UnitOfTimeValue
{
    public UnitOfTimeValue(UnitOfTime unitOfTime, decimal value)
    {
        UnitOfTime = unitOfTime;
        Value = value;
    }

    public UnitOfTime UnitOfTime { get; }

    public decimal Value { get; }

    public TimeSpan TimeSpanValue => TimeConverter.GetTimeSpan(UnitOfTime, Value);

    public string Description => $"{Common.Attributes.Description.GetDescription(UnitOfTime)}: {Value.ToHumanReadableNumber()}";
}