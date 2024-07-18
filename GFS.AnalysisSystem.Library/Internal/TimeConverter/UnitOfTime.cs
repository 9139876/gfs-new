using GFS.Common.Attributes;

namespace GFS.AnalysisSystem.Library.Internal.TimeConverter;

internal enum UnitOfTime
{
    [Description("Календарная секунда")]
    CalendarSecond = 1,

    [Description("Календарная минута")]
    CalendarMinute = 2,

    [Description("Календарный час")]
    CalendarHour = 3,

    [Description("Календарный день")]
    CalendarDay = 4,

    [Description("Календарная неделя")]
    CalendarWeek = 5,

    [Description("Календарный месяц")]
    CalendarMonth = 6,

    [Description("Календарный год")]
    CalendarYear = 7,
}