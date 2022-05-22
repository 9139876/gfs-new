using GFS.Common.Attributes;

namespace GFS.GraalCommon.Enums
{
    /// <summary>
    /// Таймфрейм
    /// </summary>
    public enum TimeFrameEnum
    {
        /// <summary>
        /// Тиковый
        /// </summary>
        [Description("Тиковый")]
        tick = 10,
        /// <summary>
        /// 1-минутный
        /// </summary>
        [Description("1-минутный")]
        min1 = 20,
        /// <summary>
        /// 4-минутный
        /// </summary>
        [Description("4-минутный")]
        min4 = 30,
        /// <summary>
        /// Часовой
        /// </summary>
        [Description("Часовой")]
        H1 = 40,
        /// <summary>
        /// Дневной
        /// </summary>
        [Description("Дневной")]
        D1 = 50,
        /// <summary>
        /// Недельный
        /// </summary>
        [Description("Недельный")]
        W1 = 60,
        /// <summary>
        /// Месячный
        /// </summary>
        [Description("Месячный")]
        M1 = 70,
        /// <summary>
        /// Сезонный
        /// </summary>
        [Description("Сезонный")]
        Seasonly = 80,
        /// <summary>
        /// Годовой
        /// </summary>
        [Description("Годовой")]
        Y1 = 90
    }
}
