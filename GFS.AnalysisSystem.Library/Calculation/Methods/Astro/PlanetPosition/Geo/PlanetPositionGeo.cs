using GFS.AnalysisSystem.Library.Calculation.Abstraction;
using GFS.AnalysisSystem.Library.Calculation.Models;
using GFS.AnalysisSystem.Library.Internal.AstroCommon;
using GFS.Common.Attributes;

namespace GFS.AnalysisSystem.Library.Calculation.Methods.Astro.PlanetPosition.Geo;

public abstract class PlanetPositionGeo : ForecastTreeMethod<PlanetPositionGeoGroup>
{
    protected abstract Planet Planet { get; }

    public override ForecastCalculationResult Calculate(CalculationContext context)
    {
        return PlanetPositionCommon.Calculate(context, Planet);
    }

    public override string Name => Description.GetDescription(Planet.PlanetType);
}

public class PlanetPositionGeoSun : PlanetPositionGeo
{
    protected override Planet Planet => Planets.Geo.Sun;
}

public class PlanetPositionGeoMoon : PlanetPositionGeo
{
    protected override Planet Planet => Planets.Geo.Moon;
}

public class PlanetPositionGeoMercury : PlanetPositionGeo
{
    protected override Planet Planet => Planets.Geo.Mercury;
}

public class PlanetPositionGeoVenus : PlanetPositionGeo
{
    protected override Planet Planet => Planets.Geo.Venus;
}

public class PlanetPositionGeoMars : PlanetPositionGeo
{
    protected override Planet Planet => Planets.Geo.Mars;
}

public class PlanetPositionGeoJupiter : PlanetPositionGeo
{
    protected override Planet Planet => Planets.Geo.Jupiter;
}

public class PlanetPositionGeoSaturn : PlanetPositionGeo
{
    protected override Planet Planet => Planets.Geo.Saturn;
}

public class PlanetPositionGeoUranus : PlanetPositionGeo
{
    protected override Planet Planet => Planets.Geo.Uranus;
}

public class PlanetPositionGeoNeptune : PlanetPositionGeo
{
    protected override Planet Planet => Planets.Geo.Neptune;
}

public class PlanetPositionGeoPluto : PlanetPositionGeo
{
    protected override Planet Planet => Planets.Geo.Pluto;
}

public class PlanetPositionGeoAverageVoshUzel : PlanetPositionGeo
{
    protected override Planet Planet => Planets.Geo.AverageVoshUzel;
}

public class PlanetPositionGeoTrueVoshUzel : PlanetPositionGeo
{
    protected override Planet Planet => Planets.Geo.TrueVoshUzel;
}

public class PlanetPositionGeoAverageApogeeMoon : PlanetPositionGeo
{
    protected override Planet Planet => Planets.Geo.AverageApogeeMoon;
}

public class PlanetPositionGeoTrueApogeeMoon : PlanetPositionGeo
{
    protected override Planet Planet => Planets.Geo.TrueApogeeMoon;
}