namespace GFS.Common.Extensions;

public static class DateTimeExtensions
{
    /// <summary>
    /// Получает значение начала дня для текущей даты
    /// </summary>
    public static DateTime StartOfDay(this DateTime dateTime)
        => new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, dateTime.Kind);
    
    /// <summary>
    /// Получает значение конца дня для текущей даты
    /// </summary>
    public static DateTime EndOfDay(this DateTime dateTime)
        => new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59, dateTime.Kind);
}