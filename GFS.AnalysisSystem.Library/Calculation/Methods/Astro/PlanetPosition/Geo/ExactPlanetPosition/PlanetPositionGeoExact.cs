using GFS.AnalysisSystem.Library.Calculation.Abstraction;
using GFS.AnalysisSystem.Library.Calculation.Models;
using GFS.AnalysisSystem.Library.Internal.AstroCommon;
using GFS.Common.Attributes;

namespace GFS.AnalysisSystem.Library.Calculation.Methods.Astro.PlanetPosition.Geo.ExactPlanetPosition;

public abstract class PlanetPositionGeoExact : ForecastTreeMethod<PlanetPositionGeoExactGroup>
{
    protected abstract Planet Planet { get; }

    public override ForecastCalculationResult Calculate(CalculationContext context)
    {
        return PlanetPositionCommon.Calculate(context, Planet, HarmonicTypeEnum.Exact);
    }

    public override string Name => Description.GetDescription(Planet.PlanetType);
}

public class PlanetPositionGeoExactSun : PlanetPositionGeoExact
{
    protected override Planet Planet => Planets.Geo.Sun;
}

public class PlanetPositionGeoExactMoon : PlanetPositionGeoExact
{
    protected override Planet Planet => Planets.Geo.Moon;
}

public class PlanetPositionGeoExactMercury : PlanetPositionGeoExact
{
    protected override Planet Planet => Planets.Geo.Mercury;
}

public class PlanetPositionGeoExactVenus : PlanetPositionGeoExact
{
    protected override Planet Planet => Planets.Geo.Venus;
}

public class PlanetPositionGeoExactMars : PlanetPositionGeoExact
{
    protected override Planet Planet => Planets.Geo.Mars;
}

public class PlanetPositionGeoExactJupiter : PlanetPositionGeoExact
{
    protected override Planet Planet => Planets.Geo.Jupiter;
}

public class PlanetPositionGeoExactSaturn : PlanetPositionGeoExact
{
    protected override Planet Planet => Planets.Geo.Saturn;
}

public class PlanetPositionGeoExactUranus : PlanetPositionGeoExact
{
    protected override Planet Planet => Planets.Geo.Uranus;
}

public class PlanetPositionGeoExactNeptune : PlanetPositionGeoExact
{
    protected override Planet Planet => Planets.Geo.Neptune;
}

public class PlanetPositionGeoExactPluto : PlanetPositionGeoExact
{
    protected override Planet Planet => Planets.Geo.Pluto;
}

public class PlanetPositionGeoExactAverageVoshUzel : PlanetPositionGeoExact
{
    protected override Planet Planet => Planets.Geo.AverageVoshUzel;
}

public class PlanetPositionGeoExactTrueVoshUzel : PlanetPositionGeoExact
{
    protected override Planet Planet => Planets.Geo.TrueVoshUzel;
}

public class PlanetPositionGeoExactAverageApogeeMoon : PlanetPositionGeoExact
{
    protected override Planet Planet => Planets.Geo.AverageApogeeMoon;
}

public class PlanetPositionGeoExactTrueApogeeMoon : PlanetPositionGeoExact
{
    protected override Planet Planet => Planets.Geo.TrueApogeeMoon;
}