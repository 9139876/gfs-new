using GFS.Common.Attributes;
using GFS.GrailCommon.Enums;

namespace GFS.AnalysisSystem.Library.Internal.TimeConverter;

/// <summary>
/// Конвертер единиц времени
/// </summary>
internal static class TimeConverter
{
    public static List<TimeConverterResultItem> Convert(decimal value, UnitOfTime unitOfTime)
    {
        throw new NotImplementedException();
    }

    public static TimeConverterResultItem[] Convert(decimal value, TimeFrameEnum timeFrame, CellsRange range)
    {
        return Enum.GetValues<UnitOfTime>()
            .SelectMany(unitOfTime => X(unitOfTime, value, timeFrame, range))
            .ToArray();
    }

    private static List<TimeConverterResultItem> X(UnitOfTime unitOfTime, decimal value, TimeFrameEnum timeFrame, CellsRange range)
    {
        // var timeSpan = GetTimeSpan(unitOfTime, value);
        // var valueInTimeFrameUnits = GetValueInTimeFrameUnits(timeSpan, timeFrame);

        //Уходим вниз диапазона
        while (GetValueInTimeFrameUnits(GetTimeSpan(unitOfTime, value), timeFrame) >= range.MinValue)
        {
            value /= 10;
        }

        var result = new List<TimeConverterResultItem>();

        var item = GetValueInTimeFrameUnits(GetTimeSpan(unitOfTime, value), timeFrame);

        while (item <= range.MaxValue)
        {
            if (item >= range.MinValue && item <= range.MaxValue)
                result.Add(new TimeConverterResultItem(item, $"{value} {Description.GetDescription(unitOfTime)}"));

            value *= 10;
            item = GetValueInTimeFrameUnits(GetTimeSpan(unitOfTime, value), timeFrame);
        }

        return result;

        // new TimeConverterResultItem(ushort.MaxValue, string.Empty)
    }

    //ToDo Защититься от переполнения int  
    private static TimeSpan GetTimeSpan(UnitOfTime unitOfTime, decimal value)
    {
        var seconds = unitOfTime switch
        {
            UnitOfTime.CalendarSecond => (int)Math.Round(value),
            UnitOfTime.CalendarMinute => (int)Math.Round(value * 60),
            UnitOfTime.CalendarHour => (int)Math.Round(value * 60 * 60),
            UnitOfTime.CalendarDay => (int)Math.Round(value * 60 * 60 * 24),
            UnitOfTime.CalendarWeek => (int)Math.Round(value * 60 * 60 * 24 * 7),
            UnitOfTime.CalendarMonth => (int)Math.Round(value * 60 * 60 * 24 * 30),
            UnitOfTime.CalendarYear => (int)Math.Round(value * 60 * 60 * 24 * 365.25m),
            _ => throw new ArgumentOutOfRangeException(nameof(unitOfTime), unitOfTime, null)
        };

        return new TimeSpan(0, 0, seconds);
    }

    //ToDo Защититься от переполнения ushort
    private static ushort GetValueInTimeFrameUnits(TimeSpan timeSpan, TimeFrameEnum timeFrame)
    {
        return timeFrame switch
        {
            TimeFrameEnum.tick => (ushort)timeSpan.TotalSeconds,
            TimeFrameEnum.min1 => (ushort)timeSpan.TotalMinutes,
            TimeFrameEnum.min4 => (ushort)Math.Round((decimal)timeSpan.TotalMinutes / 4),
            TimeFrameEnum.H1 => (ushort)timeSpan.TotalHours,
            TimeFrameEnum.D1 => (ushort)timeSpan.TotalDays,
            TimeFrameEnum.W1 => (ushort)Math.Round((decimal)timeSpan.TotalDays / 7),
            TimeFrameEnum.M1 => (ushort)Math.Round((decimal)timeSpan.TotalDays / 30),
            TimeFrameEnum.Seasonly => (ushort)Math.Round((decimal)timeSpan.TotalDays / 90),
            TimeFrameEnum.Y1 => (ushort)Math.Round((decimal)timeSpan.TotalDays / 365.25m),
            _ => throw new ArgumentOutOfRangeException(nameof(timeFrame), timeFrame, null)
        };
    }
}