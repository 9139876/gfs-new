using GFS.Common.Attributes;

namespace GFS.AnalysisSystem.Library.Internal.AstroCommon.Enums
{   
    public enum PlanetType
    {
        /// <summary>
        /// Солнце
        /// </summary>
        [Description("Солнце")]
        Sun = 0,

        /// <summary>
        /// Луна
        /// </summary>
        [Description("Луна")]
        Moon = 1,

        /// <summary>
        /// Меркурий
        /// </summary>
        [Description("Меркурий")]
        Mercury = 2,

        /// <summary>
        /// Венера
        /// </summary>
        [Description("Венера")]
        Venus = 3,

        /// <summary>
        /// Марс
        /// </summary>
        [Description("Марс")]
        Mars = 4,

        /// <summary>
        /// Юпитер
        /// </summary>
        [Description("Юпитер")]
        Jupiter = 5,

        /// <summary>
        /// Сатурн
        /// </summary>
        [Description("Сатурн")]
        Saturn = 6,

        /// <summary>
        /// Уран
        /// </summary>
        [Description("Уран")]
        Uranus = 7,

        /// <summary>
        /// Нептун
        /// </summary>
        [Description("Нептун")]
        Neptune = 8,

        /// <summary>
        /// Плутон
        /// </summary>
        [Description("Плутон")]
        Pluto = 9,
        
        /// <summary>
        /// Средний Восходящий узел
        /// </summary>
        [Description("Средний Восходящий узел")]
        AverageVoshUzel = 10,

        /// <summary>
        /// Истинный Восходящий узел
        /// </summary>
        [Description("Истинный Восходящий узел")]
        TrueVoshUzel = 11,
        
        /// <summary>
        /// Средний апогей Луны
        /// </summary>
        [Description("Средний апогей Луны")]
        AverageApogeeMoon = 12,

        /// <summary>
        /// Истинный апогей Луны
        /// </summary>
        [Description("Истинный апогей Луны")]
        TrueApogeeMoon = 13,
        
        /// <summary>
        /// Земля
        /// </summary>
        [Description("Земля")]
        Earth = 14
    }
}