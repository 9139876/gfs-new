using GFS.GrailCommon.Enums;

namespace GFS.AnalysisSystem.Library.Internal.TimeConverter;

/// <summary>
/// Конвертер единиц времени
/// </summary>
internal static class TimeConverter
{
    public static TimeSpan GetTimeSpan(decimal value, TimeFrameEnum timeFrame)
    {
        var (unitOfTime, correctValue) = timeFrame switch
        {
            TimeFrameEnum.min1 => (UnitOfTime.CalendarMinute, value),
            TimeFrameEnum.H1 => (UnitOfTime.CalendarHour, value),
            TimeFrameEnum.D1 => (UnitOfTime.CalendarDay, value),
            TimeFrameEnum.W1 => (UnitOfTime.CalendarWeek, value),
            TimeFrameEnum.M1 => (UnitOfTime.CalendarMonth, value),
            _ => throw new ArgumentOutOfRangeException(nameof(timeFrame), timeFrame, null)
        };

        return GetTimeSpan(unitOfTime, correctValue);
    }

    public static UnitOfTimeValue[] ConvertNumber(decimal value, TimeRange range)
    {
        if (range.IsZeroLength)
            return Array.Empty<UnitOfTimeValue>();

        return Enum.GetValues<UnitOfTime>()
            .SelectMany(unitOfTime => GetActualValuesByUnitOfTime(unitOfTime, value, range))
            .ToArray();
    }

    internal static (UnitOfTimeValue, UnitOfTimeValue)[] ConvertTimeSpan(TimeSpan timeSpan, TimeRange range)
    {
        if (range.IsZeroLength)
            return Array.Empty<(UnitOfTimeValue, UnitOfTimeValue)>();

        return timeSpan.TotalMinutes switch
        {
            < 0 => throw new InvalidOperationException("Значение продолжительности по времени должно иметь положительное значение"),
            < 5 => Array.Empty<(UnitOfTimeValue, UnitOfTimeValue)>(),
            _ => TimeSpanToUnitOfTimeValues(timeSpan)
                .SelectMany(item => ConvertNumber(item.Value, range).Select(res => (item, res)))
                .ToArray()
        };
    }

    #region internal

    internal static UnitOfTimeValue[] TimeSpanToUnitOfTimeValues(TimeSpan timeSpan)
    {
        return new[]
        {
            new UnitOfTimeValue(UnitOfTime.CalendarSecond, (decimal)timeSpan.TotalSeconds),
            new UnitOfTimeValue(UnitOfTime.CalendarMinute, (decimal)timeSpan.TotalMinutes),
            new UnitOfTimeValue(UnitOfTime.CalendarHour, (decimal)timeSpan.TotalMinutes / 60),
            new UnitOfTimeValue(UnitOfTime.CalendarDay, (decimal)timeSpan.TotalMinutes / (60 * 24)),
            new UnitOfTimeValue(UnitOfTime.CalendarWeek, (decimal)timeSpan.TotalMinutes / (60 * 24 * 7)),
            new UnitOfTimeValue(UnitOfTime.CalendarMonth, (decimal)timeSpan.TotalMinutes / (60 * 24 * 30)),
            new UnitOfTimeValue(UnitOfTime.CalendarYear, (decimal)timeSpan.TotalMinutes / (60 * 24 * 365.25m))
        };
    }

    internal static ushort TimeSpanToUnitOfTimeByTimeFrame(TimeSpan timeSpan, TimeFrameEnum timeFrame)
    {
        if (timeSpan.TotalMilliseconds < 0)
            throw new InvalidOperationException("Значение продолжительности по времени должно не должно быть отрицательным");

        var value = timeFrame switch
        {
            TimeFrameEnum.min1 => (decimal)timeSpan.TotalMinutes,
            TimeFrameEnum.H1 => (decimal)timeSpan.TotalHours,
            TimeFrameEnum.D1 => (decimal)timeSpan.TotalDays,
            TimeFrameEnum.W1 => Math.Round((decimal)timeSpan.TotalDays / 7),
            TimeFrameEnum.M1 => Math.Round((decimal)timeSpan.TotalDays / 30),
            _ => throw new ArgumentOutOfRangeException(nameof(timeFrame), timeFrame, null)
        };

        return value < ushort.MaxValue
            ? (ushort)value
            : ushort.MaxValue;
    }

    internal static IEnumerable<UnitOfTimeValue> GetActualValuesByUnitOfTime(UnitOfTime unitOfTime, decimal value, TimeRange range)
    {
        if (range.IsZeroLength)
            return Array.Empty<UnitOfTimeValue>();

        //Уходим вниз диапазона
        while (GetTimeSpan(unitOfTime, value) >= range.MinValue)
        {
            value /= 10;
        }

        var result = new List<UnitOfTimeValue>();

        var item = GetTimeSpan(unitOfTime, value);

        while (item <= range.MaxValue)
        {
            if (item >= range.MinValue && item <= range.MaxValue)
                result.Add(new UnitOfTimeValue(unitOfTime, value));

            value *= 10;
            item = GetTimeSpan(unitOfTime, value);
        }

        return result;
    }

    internal static TimeSpan GetTimeSpan(UnitOfTime unitOfTime, decimal value)
    {
        switch (value)
        {
            case <= 0:
                return TimeSpan.Zero;
            case > int.MaxValue:
                return TimeSpan.MaxValue;
            default:
            {
                var seconds = unitOfTime switch
                {
                    UnitOfTime.CalendarSecond => Math.Round(value),
                    UnitOfTime.CalendarMinute => Math.Round(value * 60),
                    UnitOfTime.CalendarHour => Math.Round(value * 60 * 60),
                    UnitOfTime.CalendarDay => Math.Round(value * 60 * 60 * 24),
                    UnitOfTime.CalendarWeek => Math.Round(value * 60 * 60 * 24 * 7),
                    UnitOfTime.CalendarMonth => Math.Round(value * 60 * 60 * 24 * 30),
                    UnitOfTime.CalendarYear => Math.Round(value * 60 * 60 * 24 * 365.25m),
                    _ => throw new ArgumentOutOfRangeException(nameof(unitOfTime), unitOfTime, null)
                };

                return seconds > int.MaxValue
                    ? TimeSpan.MaxValue
                    : new TimeSpan(0, 0, (int)seconds);
            }
        }
    }

    #endregion
}