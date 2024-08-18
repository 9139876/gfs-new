using GFS.Common.Attributes;
// ReSharper disable InconsistentNaming

namespace GFS.GrailCommon.Enums
{
    /// <summary> Таймфрейм </summary>
    public enum TimeFrameEnum
    {
        /// <summary> 1-минутный </summary>
        [Description("1-минутный")] min1 = 10,

        /// <summary> Часовой </summary>
        [Description("Часовой")] H1 = 20,

        /// <summary> Дневной </summary>
        [Description("Дневной")] D1 = 30,

        /// <summary> Недельный </summary>
        [Description("Недельный")] W1 = 40,

        /// <summary> Месячный </summary>
        [Description("Месячный")] M1 = 50
    }
}