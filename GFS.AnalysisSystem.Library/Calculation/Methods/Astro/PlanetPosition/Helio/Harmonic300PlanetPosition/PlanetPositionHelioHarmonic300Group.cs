using GFS.AnalysisSystem.Library.Calculation.Abstraction;
using GFS.Common.Attributes;

namespace GFS.AnalysisSystem.Library.Calculation.Methods.Astro.PlanetPosition.Helio.Harmonic300PlanetPosition;

public class PlanetPositionHelioHarmonic300Group: ForecastTreeGroup<PlanetPositionHelioGroup>
{
    public override string Name => Description.GetDescription(HarmonicTypeEnum.Harmonic300);
}