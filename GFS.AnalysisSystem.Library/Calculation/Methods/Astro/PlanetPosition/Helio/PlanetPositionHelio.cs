using GFS.AnalysisSystem.Library.Calculation.Abstraction;
using GFS.AnalysisSystem.Library.Calculation.Models;
using GFS.AnalysisSystem.Library.Internal.AstroCommon;
using GFS.Common.Attributes;

namespace GFS.AnalysisSystem.Library.Calculation.Methods.Astro.PlanetPosition.Helio;

public abstract class PlanetPositionHelio : ForecastTreeMethod<PlanetPositionHelioGroup>
{
    protected abstract Planet Planet { get; }

    public override ForecastCalculationResult Calculate(CalculationContext context)
    {
        return PlanetPositionCommon.Calculate(context, Planet);
    }

    public override string Name => Description.GetDescription(Planet.PlanetType);
}

public class PlanetPositionHelioMercury : PlanetPositionHelio
{
    protected override Planet Planet => Planets.Helio.Mercury;
}

public class PlanetPositionHelioVenus : PlanetPositionHelio
{
    protected override Planet Planet => Planets.Helio.Venus;
}

public class PlanetPositionHelioMars : PlanetPositionHelio
{
    protected override Planet Planet => Planets.Helio.Mars;
}

public class PlanetPositionHelioJupiter : PlanetPositionHelio
{
    protected override Planet Planet => Planets.Helio.Jupiter;
}

public class PlanetPositionHelioSaturn : PlanetPositionHelio
{
    protected override Planet Planet => Planets.Helio.Saturn;
}

public class PlanetPositionHelioUranus : PlanetPositionHelio
{
    protected override Planet Planet => Planets.Helio.Uranus;
}

public class PlanetPositionHelioNeptune : PlanetPositionHelio
{
    protected override Planet Planet => Planets.Helio.Neptune;
}

public class PlanetPositionHelioPluto : PlanetPositionHelio
{
    protected override Planet Planet => Planets.Helio.Pluto;
}

public class PlanetPositionHelioEarth : PlanetPositionHelio
{
    protected override Planet Planet => Planets.Helio.Earth;
}