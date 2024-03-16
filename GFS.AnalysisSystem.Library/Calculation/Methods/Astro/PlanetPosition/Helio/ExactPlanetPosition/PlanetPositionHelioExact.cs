using GFS.AnalysisSystem.Library.Calculation.Abstraction;
using GFS.AnalysisSystem.Library.Calculation.Models;
using GFS.AnalysisSystem.Library.Internal.AstroCommon;
using GFS.Common.Attributes;

namespace GFS.AnalysisSystem.Library.Calculation.Methods.Astro.PlanetPosition.Helio.ExactPlanetPosition;

public abstract class PlanetPositionHelioExact : ForecastTreeMethod<PlanetPositionHelioExactGroup>
{
    protected abstract Planet Planet { get; }

    public override ForecastCalculationResult Calculate(CalculationContext context)
    {
        return PlanetPositionCommon.Calculate(context, Planet, HarmonicTypeEnum.Exact);
    }

    public override string Name => Description.GetDescription(Planet.PlanetType);
}

public class PlanetPositionHelioExactMercury : PlanetPositionHelioExact
{
    protected override Planet Planet => Planets.Helio.Mercury;
}

public class PlanetPositionHelioExactVenus : PlanetPositionHelioExact
{
    protected override Planet Planet => Planets.Helio.Venus;
}

public class PlanetPositionHelioExactMars : PlanetPositionHelioExact
{
    protected override Planet Planet => Planets.Helio.Mars;
}

public class PlanetPositionHelioExactJupiter : PlanetPositionHelioExact
{
    protected override Planet Planet => Planets.Helio.Jupiter;
}

public class PlanetPositionHelioExactSaturn : PlanetPositionHelioExact
{
    protected override Planet Planet => Planets.Helio.Saturn;
}

public class PlanetPositionHelioExactUranus : PlanetPositionHelioExact
{
    protected override Planet Planet => Planets.Helio.Uranus;
}

public class PlanetPositionHelioExactNeptune : PlanetPositionHelioExact
{
    protected override Planet Planet => Planets.Helio.Neptune;
}

public class PlanetPositionHelioExactPluto : PlanetPositionHelioExact
{
    protected override Planet Planet => Planets.Helio.Pluto;
}

public class PlanetPositionHelioExactEarth : PlanetPositionHelioExact
{
    protected override Planet Planet => Planets.Helio.Earth;
}