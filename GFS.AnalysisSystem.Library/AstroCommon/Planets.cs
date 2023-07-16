using GFS.AnalysisSystem.Library.AstroCommon.Enums;
using GFS.Common.Attributes;

namespace GFS.AnalysisSystem.Library.AstroCommon
{
    public class Planet
    {
        public Planet(PlanetType planetType, bool helio)
        {
            PlanetType = planetType;
            Helio = helio;
        }
        
        /// <summary>
        /// Выводимое имя планеты
        /// </summary>
        public string Name => Description.GetDescription(PlanetType) + (Helio ? "_Helio" : "_Geo");

        /// <summary>
        /// Система - Гелио или Гео
        /// </summary>
        public bool Helio { get; }

        /// <summary>
        /// Планета
        /// </summary>
        public PlanetType PlanetType { get; }
    }

    public static class Planets
    {
        public static class Geo
        {
            private const bool IS_HELIO = false;

            public static Planet Sun => new (PlanetType.Sun, IS_HELIO);
            public static Planet Moon => new (PlanetType.Moon, IS_HELIO);
            public static Planet Mercury => new (PlanetType.Mercury, IS_HELIO);
            public static Planet Venus => new (PlanetType.Venus, IS_HELIO);
            public static Planet Mars => new (PlanetType.Mars, IS_HELIO);
            public static Planet Jupiter => new (PlanetType.Jupiter, IS_HELIO);
            public static Planet Saturn => new (PlanetType.Saturn, IS_HELIO);
            public static Planet Uranus => new (PlanetType.Uranus, IS_HELIO);
            public static Planet Neptune => new (PlanetType.Neptune, IS_HELIO);
            public static Planet Pluto => new (PlanetType.Pluto, IS_HELIO);
            public static Planet AverageVoshUzel => new (PlanetType.AverageVoshUzel, IS_HELIO);
            public static Planet TrueVoshUzel => new (PlanetType.TrueVoshUzel, IS_HELIO);
            public static Planet AverageApogeeMoon => new (PlanetType.AverageApogeeMoon, IS_HELIO);
            public static Planet TrueApogeeMoon => new (PlanetType.TrueApogeeMoon, IS_HELIO);
        }
        
        public static class Helio
        {
            private const bool IS_HELIO = true;

            public static Planet Sun => new (PlanetType.Sun, IS_HELIO);
            public static Planet Moon => new (PlanetType.Moon, IS_HELIO);
            public static Planet Mercury => new (PlanetType.Mercury, IS_HELIO);
            public static Planet Venus => new (PlanetType.Venus, IS_HELIO);
            public static Planet Mars => new (PlanetType.Mars, IS_HELIO);
            public static Planet Jupiter => new (PlanetType.Jupiter, IS_HELIO);
            public static Planet Saturn => new (PlanetType.Saturn, IS_HELIO);
            public static Planet Uranus => new (PlanetType.Uranus, IS_HELIO);
            public static Planet Neptune => new (PlanetType.Neptune, IS_HELIO);
            public static Planet Pluto => new (PlanetType.Pluto, IS_HELIO);
            public static Planet Earth => new (PlanetType.Earth, IS_HELIO);
        }
    }
}