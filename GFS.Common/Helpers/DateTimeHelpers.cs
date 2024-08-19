namespace GFS.Common.Helpers;

public static class DateTimeHelpers
{
    /// <summary>
    /// Возвращает среднее значение между двумя датами
    /// </summary>
    /// <param name="dt1">Первая дата</param>
    /// <param name="dt2">Вторая дата</param>
    public static DateTime MedianDate(DateTime dt1, DateTime dt2)
    {
        if (dt1 == dt2)
            return dt1;

        var min = new DateTime(Math.Min(dt1.Ticks, dt2.Ticks));
        var max = new DateTime(Math.Max(dt1.Ticks, dt2.Ticks));

        return new DateTime(min.Ticks + (max.Ticks - min.Ticks) / 2);
    }

    /// <summary>
    /// Возвращает наименьшую из дат
    /// </summary>
    /// <param name="dates">Даты</param>
    /// <exception cref="ArgumentException">Если не передано ни одной даты</exception>
    public static DateTime MinDate(params DateTime[] dates)
    {
        if (!dates.Any())
            throw new ArgumentException("В метод не передано ни одной даты", nameof(dates));
        var result = dates[0];

        foreach (var date in dates.Skip(1))
            if (date < result)
                result = date;

        return result;
    }

    /// <summary>
    /// Возвращает наибольшую из дат
    /// </summary>
    /// <param name="dates">Даты</param>
    /// <exception cref="ArgumentException">Если не передано ни одной даты</exception>
    public static DateTime MaxDate(params DateTime[] dates)
    {
        if (!dates.Any())
            throw new ArgumentException("В метод не передано ни одной даты", nameof(dates));
        var result = dates[0];

        foreach (var date in dates.Skip(1))
            if (date > result)
                result = date;

        return result;
    }
}