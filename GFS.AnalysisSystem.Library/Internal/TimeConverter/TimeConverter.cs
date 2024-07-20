using GFS.Common.Attributes;
using GFS.GrailCommon.Enums;

namespace GFS.AnalysisSystem.Library.Internal.TimeConverter;

/// <summary>
/// Конвертер единиц времени
/// </summary>
internal static class TimeConverter
{
    public static ushort TimeSpanToUnitOfTimeByTimeFrame(TimeSpan timeSpan, TimeFrameEnum timeFrame)
    {
        if (timeSpan.TotalMilliseconds < 0)
            throw new InvalidOperationException("Значение продолжительности по времени должно не должно быть отрицательным");

        var value = timeFrame switch
        {
            TimeFrameEnum.tick => (decimal)timeSpan.TotalSeconds,
            TimeFrameEnum.min1 => (decimal)timeSpan.TotalMinutes,
            TimeFrameEnum.min4 => Math.Round((decimal)timeSpan.TotalMinutes / 4),
            TimeFrameEnum.H1 => (decimal)timeSpan.TotalHours,
            TimeFrameEnum.D1 => (decimal)timeSpan.TotalDays,
            TimeFrameEnum.W1 => Math.Round((decimal)timeSpan.TotalDays / 7),
            TimeFrameEnum.M1 => Math.Round((decimal)timeSpan.TotalDays / 30),
            TimeFrameEnum.Seasonly => Math.Round((decimal)timeSpan.TotalDays / 90),
            TimeFrameEnum.Y1 => Math.Round((decimal)timeSpan.TotalDays / 365.25m),
            _ => throw new ArgumentOutOfRangeException(nameof(timeFrame), timeFrame, null)
        };

        return value < ushort.MaxValue
            ? (ushort)value
            : ushort.MaxValue;
    }

    public static TimeSpan GetTimeSpan(decimal value, TimeFrameEnum timeFrame)
    {
        var (unitOfTime, correctValue) = timeFrame switch
        {
            TimeFrameEnum.tick => (UnitOfTime.CalendarSecond, value),
            TimeFrameEnum.min1 => (UnitOfTime.CalendarMinute, value),
            TimeFrameEnum.min4 => (UnitOfTime.CalendarSecond, value * 4),
            TimeFrameEnum.H1 => (UnitOfTime.CalendarHour, value),
            TimeFrameEnum.D1 => (UnitOfTime.CalendarDay, value),
            TimeFrameEnum.W1 => (UnitOfTime.CalendarWeek, value),
            TimeFrameEnum.M1 => (UnitOfTime.CalendarMonth, value),
            TimeFrameEnum.Seasonly => (UnitOfTime.CalendarMonth, value * 3),
            TimeFrameEnum.Y1 => (UnitOfTime.CalendarYear, value),
            _ => throw new ArgumentOutOfRangeException(nameof(timeFrame), timeFrame, null)
        };

        return GetTimeSpan(unitOfTime, correctValue);
    }

    public static TimeConverterResultItem[] ConvertTimeSpan(TimeSpan timeSpan, TimeRange range)
    {
        if (range.IsZeroLength)
            return Array.Empty<TimeConverterResultItem>();

        return timeSpan.Minutes switch
        {
            < 0 => throw new InvalidOperationException("Значение продолжительности по времени должно иметь положительное значение"),
            < 5 => Array.Empty<TimeConverterResultItem>(),
            _ => new[]
                {
                    (decimal)timeSpan.TotalSeconds,
                    (decimal)timeSpan.TotalMinutes,
                    (decimal)timeSpan.TotalMinutes / 60,
                    (decimal)timeSpan.TotalMinutes / (60 * 24),
                    (decimal)timeSpan.TotalMinutes / (60 * 24 * 7),
                    (decimal)timeSpan.TotalMinutes / (60 * 24 * 30),
                    (decimal)timeSpan.TotalMinutes / (60 * 24 * 365.25m),
                }
                .SelectMany(item => ConvertNumber(item, range))
                .ToArray()
        };
    }

    public static TimeConverterResultItem[] ConvertNumber(decimal value, TimeRange range)
    {
        if (range.IsZeroLength)
            return Array.Empty<TimeConverterResultItem>();

        return Enum.GetValues<UnitOfTime>()
            .SelectMany(unitOfTime => GetActualValuesByUnitOfTime(unitOfTime, value, range))
            .ToArray();
    }

    internal static IEnumerable<TimeConverterResultItem> GetActualValuesByUnitOfTime(UnitOfTime unitOfTime, decimal value, TimeRange range)
    {
        if (range.IsZeroLength)
            return Array.Empty<TimeConverterResultItem>();

        //Уходим вниз диапазона
        while (GetTimeSpan(unitOfTime, value) >= range.MinValue)
        {
            value /= 10;
        }

        var result = new List<TimeConverterResultItem>();

        var item = GetTimeSpan(unitOfTime, value);

        while (item <= range.MaxValue)
        {
            if (item >= range.MinValue && item <= range.MaxValue)
                result.Add(new TimeConverterResultItem(item, $"{Description.GetDescription(unitOfTime)}: {value:0.##################}"));

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
}