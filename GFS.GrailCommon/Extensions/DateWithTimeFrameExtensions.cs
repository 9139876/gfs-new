﻿using GFS.GrailCommon.Enums;

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
                TimeFrameEnum.min1 => date.AddMinutes(increment),
                TimeFrameEnum.H1 => date.AddHours(increment),
                TimeFrameEnum.D1 => date.AddDays(increment),
                TimeFrameEnum.W1 => date.AddDays(increment * 7),
                TimeFrameEnum.M1 => date.AddMonths(increment),

                _ => throw new ArgumentException($"Неподходящий таймфрейм - {timeFrame}", nameof(timeFrame)),
            };
        }

        public static bool EqualsForTimeFrame(this DateTime dt1, DateTime dt2, TimeFrameEnum timeFrame)
            => DateWithTimeFrameHelpers.DatesDifferent(dt1, dt2, timeFrame) == 0;

        /// <summary>
        /// Приведение даты к единому формату - усреднение незначащих разрядов
        /// </summary>
        /// <param name="date"> Дата </param>
        /// <param name="timeFrame"> Таймфрейм </param>
        /// <param name="kind">Тип даты (локальная, UTC...)</param>
        public static DateTime CorrectDateByTf(this DateTime date, TimeFrameEnum timeFrame, DateTimeKind? kind = null)
        {
            int year = date.Year,
                month = date.Month,
                day = date.Day,
                hour = date.Hour,
                min = date.Minute,
                sec;

            switch (timeFrame)
            {
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
                case TimeFrameEnum.min1:
                    sec = 30;
                    break;

                default:
                    throw new NotSupportedException($"Not supported timeframe '{timeFrame}'");
            }

            return new DateTime(
                year: year,
                month: month,
                day: day,
                hour: hour,
                minute: min,
                second: sec,
                kind: kind ?? date.Kind);
        }

        /// <summary>
        /// Получение строки даты в зависимости от таймфрейма
        /// </summary>
        /// <param name="date"> Дата </param>
        /// <param name="timeFrame"> Таймфрейм </param>
        public static string GetDateStringByTimeFrame(this DateTime date, TimeFrameEnum timeFrame)
        {
            return timeFrame switch
            {
                TimeFrameEnum.min1 or TimeFrameEnum.H1 => date.ToString("dd.MM.yyyy hh:mm"),
                TimeFrameEnum.D1 or TimeFrameEnum.W1 or TimeFrameEnum.M1 => date.ToString("dd.MM.yyyy"),
                _ => throw new ArgumentOutOfRangeException(nameof(timeFrame), timeFrame, null)
            };
        }
    }

    public static class DateWithTimeFrameHelpers
    {
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
                TimeFrameEnum.min1 => (int)diff.TotalMinutes,
                TimeFrameEnum.H1 => (int)diff.TotalHours,
                TimeFrameEnum.D1 => (int)diff.TotalDays,
                TimeFrameEnum.W1 => (int)(diff.TotalDays / 7),
                TimeFrameEnum.M1 => (int)(diff.TotalDays / 30),

                _ => throw new NotSupportedException($"Not supported timeframe '{timeFrame}'")
            };
        }
    }
}