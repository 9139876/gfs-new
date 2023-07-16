using GFS.Common.Attributes;

namespace GFS.AnalysisSystem.Library.AstroCommon.Enums
{
    public enum PlanetParameterType
    {
        /// <summary>
        /// Долгота
        /// </summary>
        [Description("Долгота")]
        Longitude = 0,
        
        /// <summary>
        /// Широта
        /// </summary>
        [Description("Широта")]
        Latitude = 1,
        
        /// <summary>
        /// Расстояние
        /// </summary>
        [Description("Расстояние")]
        Distance = 2,
        
        /// <summary>
        /// Скорость изменения долготы
        /// </summary>
        [Description("Скорость изменения долготы")]
        SpeedInLongitude = 3,
        
        /// <summary>
        /// Скорость изменения широты
        /// </summary>
        [Description("Скорость изменения широты")]
        SpeedInLatitude = 4,
        
        /// <summary>
        /// Скорость изменения расстояния
        /// </summary>
        [Description("Скорость изменения расстояния")]
        SpeedInDistance = 5
    }
}