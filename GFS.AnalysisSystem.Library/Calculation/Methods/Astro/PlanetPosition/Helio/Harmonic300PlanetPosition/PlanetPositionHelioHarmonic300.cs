using GFS.AnalysisSystem.Library.Calculation.Abstraction;
using GFS.AnalysisSystem.Library.Calculation.Models;
using GFS.AnalysisSystem.Library.Internal.AstroCommon;
using GFS.Common.Attributes;

namespace GFS.AnalysisSystem.Library.Calculation.Methods.Astro.PlanetPosition.Helio.Harmonic300PlanetPosition;

public abstract class PlanetPositionHelioHarmonic300 : ForecastTreeMethod<PlanetPositionHelioHarmonic300Group>
{
    protected abstract Planet Planet { get; }

    public override ForecastCalculationResult Calculate(CalculationContext context)
    {
        return PlanetPositionCommon.Calculate(context, Planet, HarmonicTypeEnum.Harmonic300);
    }

    public override string Name => Description.GetDescription(Planet.PlanetType);
}

public class PlanetPositionHelioHarmonic300Mercury : PlanetPositionHelioHarmonic300
{
    protected override Planet Planet => Planets.Helio.Mercury;
}

public class PlanetPositionHelioHarmonic300Venus : PlanetPositionHelioHarmonic300
{
    protected override Planet Planet => Planets.Helio.Venus;
}

public class PlanetPositionHelioHarmonic300Mars : PlanetPositionHelioHarmonic300
{
    protected override Planet Planet => Planets.Helio.Mars;
}

public class PlanetPositionHelioHarmonic300Jupiter : PlanetPositionHelioHarmonic300
{
    protected override Planet Planet => Planets.Helio.Jupiter;
}

public class PlanetPositionHelioHarmonic300Saturn : PlanetPositionHelioHarmonic300
{
    protected override Planet Planet => Planets.Helio.Saturn;
}

public class PlanetPositionHelioHarmonic300Uranus : PlanetPositionHelioHarmonic300
{
    protected override Planet Planet => Planets.Helio.Uranus;
}

public class PlanetPositionHelioHarmonic300Neptune : PlanetPositionHelioHarmonic300
{
    protected override Planet Planet => Planets.Helio.Neptune;
}

public class PlanetPositionHelioHarmonic300Pluto : PlanetPositionHelioHarmonic300
{
    protected override Planet Planet => Planets.Helio.Pluto;
}

public class PlanetPositionHelioHarmonic300Earth : PlanetPositionHelioHarmonic300
{
    protected override Planet Planet => Planets.Helio.Earth;
}