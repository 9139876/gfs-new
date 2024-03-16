using GFS.AnalysisSystem.Library.Calculation.Abstraction;
using GFS.AnalysisSystem.Library.Calculation.Models;
using GFS.AnalysisSystem.Library.Internal.AstroCommon;
using GFS.Common.Attributes;

namespace GFS.AnalysisSystem.Library.Calculation.Methods.Astro.PlanetPosition.Helio.Harmonic60PlanetPosition;

public abstract class PlanetPositionHelioHarmonic60 : ForecastTreeMethod<PlanetPositionHelioHarmonic60Group>
{
    protected abstract Planet Planet { get; }

    public override ForecastCalculationResult Calculate(CalculationContext context)
    {
        return PlanetPositionCommon.Calculate(context, Planet, HarmonicTypeEnum.Harmonic60);
    }

    public override string Name => Description.GetDescription(Planet.PlanetType);
}

public class PlanetPositionHelioHarmonic60Mercury : PlanetPositionHelioHarmonic60
{
    protected override Planet Planet => Planets.Helio.Mercury;
}

public class PlanetPositionHelioHarmonic60Venus : PlanetPositionHelioHarmonic60
{
    protected override Planet Planet => Planets.Helio.Venus;
}

public class PlanetPositionHelioHarmonic60Mars : PlanetPositionHelioHarmonic60
{
    protected override Planet Planet => Planets.Helio.Mars;
}

public class PlanetPositionHelioHarmonic60Jupiter : PlanetPositionHelioHarmonic60
{
    protected override Planet Planet => Planets.Helio.Jupiter;
}

public class PlanetPositionHelioHarmonic60Saturn : PlanetPositionHelioHarmonic60
{
    protected override Planet Planet => Planets.Helio.Saturn;
}

public class PlanetPositionHelioHarmonic60Uranus : PlanetPositionHelioHarmonic60
{
    protected override Planet Planet => Planets.Helio.Uranus;
}

public class PlanetPositionHelioHarmonic60Neptune : PlanetPositionHelioHarmonic60
{
    protected override Planet Planet => Planets.Helio.Neptune;
}

public class PlanetPositionHelioHarmonic60Pluto : PlanetPositionHelioHarmonic60
{
    protected override Planet Planet => Planets.Helio.Pluto;
}

public class PlanetPositionHelioHarmonic60Earth : PlanetPositionHelioHarmonic60
{
    protected override Planet Planet => Planets.Helio.Earth;
}