using GFS.GrailCommon.Enums;

namespace GFS.GrailCommon.Extensions
{
    public static class DateWithTimeFrameExtensions
    {
        /// <summary>
        /// Возвращает дату, отстоящую от заданной на указанное число единиц времени таймфрейма.
        /// </summary>
        /// <param name="date">Заданная дата.</param>
        /// <param name="timeFrame">Таймфрейм.</param>
        /// <param name="increment">Добавляемое число единиц времени таймфрейма.</param>
        /// <returns>Дата, отстоящая от заданной на указанное число единиц времени таймфрейма</returns>
        public static DateTime AddDate(this DateTime date, TimeFrameEnum timeFrame, int increment)
        {
            return timeFrame switch
            {
                (TimeFrameEnum.min1) => date.AddMinutes(increment),
                (TimeFrameEnum.min4) => date.AddMinutes(increment * 4),
                (TimeFrameEnum.H1) => date.AddHours(increment),
                (TimeFrameEnum.D1) => date.AddDays(increment),
                (TimeFrameEnum.W1) => date.AddDays(increment * 7),
                (TimeFrameEnum.M1) => date.AddMonths(increment),
                (TimeFrameEnum.Seasonly) => date.AddMonths(increment * 3),
                (TimeFrameEnum.Y1) => date.AddYears(increment),

                _ => throw new ArgumentException($"Неподходящий таймфрейм - {timeFrame}", nameof(timeFrame)),
            };
        }

        /// <summary>
        /// Возвращает разницу между датами в единицах времени таймфрейма.
        /// </summary>
        /// <param name="dt1">Дата 1.</param>
        /// <param name="dt2">Дата 2.</param>
        /// <param name="timeFrame">Таймфрейм.</param>
        /// <returns>Разница между датами в единицах времени таймфрейма.</returns>
        public static int DatesDifferent(DateTime dt1, DateTime dt2, TimeFrameEnum timeFrame)
        {
            var diff = new DateTime(Math.Max(dt1.Ticks, dt2.Ticks)) - new DateTime(Math.Min(dt1.Ticks, dt2.Ticks));

            return timeFrame switch
            {
                TimeFrameEnum.tick => (int)diff.TotalSeconds,
                TimeFrameEnum.min1 => (int)diff.TotalMinutes,
                TimeFrameEnum.min4 => (int)(diff.TotalMinutes / 4),
                TimeFrameEnum.H1 => (int)diff.TotalHours,
                TimeFrameEnum.D1 => (int)diff.TotalDays,
                TimeFrameEnum.W1 => (int)(diff.TotalDays / 7),
                TimeFrameEnum.M1 => (int)(diff.TotalDays / 30),
                TimeFrameEnum.Seasonly => (int)(diff.TotalDays / 120),
                TimeFrameEnum.Y1 => (int)(diff.TotalDays / 365.25),

                _ => throw new NotSupportedException($"Not supported timeframe '{timeFrame}'")
            };
        }

        public static bool EqualsForTimeFrame(this DateTime dt1, DateTime dt2, TimeFrameEnum timeFrame)
            => DatesDifferent(dt1, dt2, timeFrame) == 0;

        /// <summary>
        /// Приведение даты к единому формату - усреднение незначащих разрядов
        /// </summary>
        /// <param name="date"></param>
        /// <param name="timeFrame"></param>
        /// <returns></returns>
        public static DateTime CorrectDateByTf(this DateTime date, TimeFrameEnum timeFrame)
        {
            int year = date.Year,
                month = date.Month,
                day = date.Day,
                hour = date.Hour,
                min = date.Minute,
                sec = date.Second;

            switch (timeFrame)
            {
                case TimeFrameEnum.Y1:
                    month = 6;
                    day = 15;
                    hour = 12;
                    min = 0;
                    sec = 0;
                    break;
                case TimeFrameEnum.Seasonly:
                {
                    if (date.Month <= 6)
                        month = date.Month <= 3 ? 2 : 5;
                    else
                        month = date.Month <= 9 ? 8 : 11;

                    day = 15;
                    hour = 12;
                    min = 0;
                    break;
                }
                case TimeFrameEnum.M1:
                    day = 15;
                    hour = 12;
                    min = 0;
                    sec = 0;
                    break;
                case TimeFrameEnum.W1:
                {
                    date = date.DayOfWeek switch
                    {
                        DayOfWeek.Monday => date.AddDays(2),
                        DayOfWeek.Tuesday => date.AddDays(1),
                        DayOfWeek.Wednesday => date.AddDays(0),
                        DayOfWeek.Thursday => date.AddDays(-1),
                        DayOfWeek.Friday => date.AddDays(-2),
                        DayOfWeek.Saturday => date.AddDays(-3),
                        DayOfWeek.Sunday => date.AddDays(-4),

                        _ => date
                    };
                    year = date.Year;
                    month = date.Month;
                    day = date.Day;
                    hour = 12;
                    min = 0;
                    sec = 0;
                    break;
                }
                case TimeFrameEnum.D1:
                    hour = 12;
                    min = 0;
                    sec = 0;
                    break;
                case TimeFrameEnum.H1:
                    min = 30;
                    sec = 0;
                    break;
                case TimeFrameEnum.min4:
                    min = (int)Math.Floor((double)min / 4) * 4 + 2;
                    sec = 0;
                    break;
                case TimeFrameEnum.min1:
                    sec = 30;
                    break;
                case TimeFrameEnum.tick:
                    break;

                default:
                    throw new NotSupportedException($"Not supported timeframe '{timeFrame}'");
            }

            return new DateTime(
                year,
                month,
                day,
                hour,
                min,
                sec,
                date.Kind);
        }

        /// <summary>
        /// Возвращает среднее значение между двумя датами
        /// </summary>
        /// <param name="dt1">Первая дата</param>
        /// <param name="dt2">Вторая дата</param>
        /// <returns></returns>
        public static DateTime GetMedian(DateTime dt1, DateTime dt2)
        {
            if (dt1 == dt2)
                return dt1;

            var min = new DateTime(Math.Min(dt1.Ticks, dt2.Ticks));
            var max = new DateTime(Math.Max(dt1.Ticks, dt2.Ticks));

            return new DateTime(min.Ticks + (max.Ticks - min.Ticks) / 2);
        }
    }
}