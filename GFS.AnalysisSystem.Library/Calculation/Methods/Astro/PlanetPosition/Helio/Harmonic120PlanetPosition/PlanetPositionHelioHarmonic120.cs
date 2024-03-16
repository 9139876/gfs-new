using GFS.AnalysisSystem.Library.Calculation.Abstraction;
using GFS.AnalysisSystem.Library.Calculation.Models;
using GFS.AnalysisSystem.Library.Internal.AstroCommon;
using GFS.Common.Attributes;

namespace GFS.AnalysisSystem.Library.Calculation.Methods.Astro.PlanetPosition.Helio.Harmonic120PlanetPosition;

public abstract class PlanetPositionHelioHarmonic120 : ForecastTreeMethod<PlanetPositionHelioHarmonic120Group>
{
    protected abstract Planet Planet { get; }

    public override ForecastCalculationResult Calculate(CalculationContext context)
    {
        return PlanetPositionCommon.Calculate(context, Planet, HarmonicTypeEnum.Harmonic120);
    }

    public override string Name => Description.GetDescription(Planet.PlanetType);
}

public class PlanetPositionHelioHarmonic120Mercury : PlanetPositionHelioHarmonic120
{
    protected override Planet Planet => Planets.Helio.Mercury;
}

public class PlanetPositionHelioHarmonic120Venus : PlanetPositionHelioHarmonic120
{
    protected override Planet Planet => Planets.Helio.Venus;
}

public class PlanetPositionHelioHarmonic120Mars : PlanetPositionHelioHarmonic120
{
    protected override Planet Planet => Planets.Helio.Mars;
}

public class PlanetPositionHelioHarmonic120Jupiter : PlanetPositionHelioHarmonic120
{
    protected override Planet Planet => Planets.Helio.Jupiter;
}

public class PlanetPositionHelioHarmonic120Saturn : PlanetPositionHelioHarmonic120
{
    protected override Planet Planet => Planets.Helio.Saturn;
}

public class PlanetPositionHelioHarmonic120Uranus : PlanetPositionHelioHarmonic120
{
    protected override Planet Planet => Planets.Helio.Uranus;
}

public class PlanetPositionHelioHarmonic120Neptune : PlanetPositionHelioHarmonic120
{
    protected override Planet Planet => Planets.Helio.Neptune;
}

public class PlanetPositionHelioHarmonic120Pluto : PlanetPositionHelioHarmonic120
{
    protected override Planet Planet => Planets.Helio.Pluto;
}

public class PlanetPositionHelioHarmonic120Earth : PlanetPositionHelioHarmonic120
{
    protected override Planet Planet => Planets.Helio.Earth;
}