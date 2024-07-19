using GFS.Common.Attributes;

namespace GFS.AnalysisSystem.Library.Internal.TimeConverter;

internal enum UnitOfTime
{
    [Description("Календарные секунды")]
    CalendarSecond = 1,

    [Description("Календарные минуты")]
    CalendarMinute = 2,

    [Description("Календарные часы")]
    CalendarHour = 3,

    [Description("Календарные дни")]
    CalendarDay = 4,

    [Description("Календарные недели")]
    CalendarWeek = 5,

    [Description("Календарные месяцы")]
    CalendarMonth = 6,

    [Description("Календарные годы")]
    CalendarYear = 7,
}