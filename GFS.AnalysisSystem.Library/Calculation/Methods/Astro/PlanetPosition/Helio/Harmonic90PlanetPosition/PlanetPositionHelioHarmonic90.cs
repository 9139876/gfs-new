using GFS.AnalysisSystem.Library.Calculation.Abstraction;
using GFS.AnalysisSystem.Library.Calculation.Models;
using GFS.AnalysisSystem.Library.Internal.AstroCommon;
using GFS.Common.Attributes;

namespace GFS.AnalysisSystem.Library.Calculation.Methods.Astro.PlanetPosition.Helio.Harmonic90PlanetPosition;

public abstract class PlanetPositionHelioHarmonic90 : ForecastTreeMethod<PlanetPositionHelioHarmonic90Group>
{
    protected abstract Planet Planet { get; }

    public override ForecastCalculationResult Calculate(CalculationContext context)
    {
        return PlanetPositionCommon.Calculate(context, Planet, HarmonicTypeEnum.Harmonic90);
    }

    public override string Name => Description.GetDescription(Planet.PlanetType);
}

public class PlanetPositionHelioHarmonic90Mercury : PlanetPositionHelioHarmonic90
{
    protected override Planet Planet => Planets.Helio.Mercury;
}

public class PlanetPositionHelioHarmonic90Venus : PlanetPositionHelioHarmonic90
{
    protected override Planet Planet => Planets.Helio.Venus;
}

public class PlanetPositionHelioHarmonic90Mars : PlanetPositionHelioHarmonic90
{
    protected override Planet Planet => Planets.Helio.Mars;
}

public class PlanetPositionHelioHarmonic90Jupiter : PlanetPositionHelioHarmonic90
{
    protected override Planet Planet => Planets.Helio.Jupiter;
}

public class PlanetPositionHelioHarmonic90Saturn : PlanetPositionHelioHarmonic90
{
    protected override Planet Planet => Planets.Helio.Saturn;
}

public class PlanetPositionHelioHarmonic90Uranus : PlanetPositionHelioHarmonic90
{
    protected override Planet Planet => Planets.Helio.Uranus;
}

public class PlanetPositionHelioHarmonic90Neptune : PlanetPositionHelioHarmonic90
{
    protected override Planet Planet => Planets.Helio.Neptune;
}

public class PlanetPositionHelioHarmonic90Pluto : PlanetPositionHelioHarmonic90
{
    protected override Planet Planet => Planets.Helio.Pluto;
}

public class PlanetPositionHelioHarmonic90Earth : PlanetPositionHelioHarmonic90
{
    protected override Planet Planet => Planets.Helio.Earth;
}