using GFS.AnalysisSystem.Library.Calculation.Abstraction;
using GFS.AnalysisSystem.Library.Calculation.Models;
using GFS.AnalysisSystem.Library.Internal.AstroCommon;
using GFS.Common.Attributes;

namespace GFS.AnalysisSystem.Library.Calculation.Methods.Astro.PlanetPosition.Helio.Harmonic270PlanetPosition;

public abstract class PlanetPositionHelioHarmonic270 : ForecastTreeMethod<PlanetPositionHelioHarmonic270Group>
{
    protected abstract Planet Planet { get; }

    public override ForecastCalculationResult Calculate(CalculationContext context)
    {
        return PlanetPositionCommon.Calculate(context, Planet, HarmonicTypeEnum.Harmonic270);
    }

    public override string Name => Description.GetDescription(Planet.PlanetType);
}

public class PlanetPositionHelioHarmonic270Mercury : PlanetPositionHelioHarmonic270
{
    protected override Planet Planet => Planets.Helio.Mercury;
}

public class PlanetPositionHelioHarmonic270Venus : PlanetPositionHelioHarmonic270
{
    protected override Planet Planet => Planets.Helio.Venus;
}

public class PlanetPositionHelioHarmonic270Mars : PlanetPositionHelioHarmonic270
{
    protected override Planet Planet => Planets.Helio.Mars;
}

public class PlanetPositionHelioHarmonic270Jupiter : PlanetPositionHelioHarmonic270
{
    protected override Planet Planet => Planets.Helio.Jupiter;
}

public class PlanetPositionHelioHarmonic270Saturn : PlanetPositionHelioHarmonic270
{
    protected override Planet Planet => Planets.Helio.Saturn;
}

public class PlanetPositionHelioHarmonic270Uranus : PlanetPositionHelioHarmonic270
{
    protected override Planet Planet => Planets.Helio.Uranus;
}

public class PlanetPositionHelioHarmonic270Neptune : PlanetPositionHelioHarmonic270
{
    protected override Planet Planet => Planets.Helio.Neptune;
}

public class PlanetPositionHelioHarmonic270Pluto : PlanetPositionHelioHarmonic270
{
    protected override Planet Planet => Planets.Helio.Pluto;
}

public class PlanetPositionHelioHarmonic270Earth : PlanetPositionHelioHarmonic270
{
    protected override Planet Planet => Planets.Helio.Earth;
}