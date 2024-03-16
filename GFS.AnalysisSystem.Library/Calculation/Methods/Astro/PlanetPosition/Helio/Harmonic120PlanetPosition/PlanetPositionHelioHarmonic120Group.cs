using GFS.AnalysisSystem.Library.Calculation.Abstraction;
using GFS.Common.Attributes;

namespace GFS.AnalysisSystem.Library.Calculation.Methods.Astro.PlanetPosition.Helio.Harmonic120PlanetPosition;

public class PlanetPositionHelioHarmonic120Group: ForecastTreeGroup<PlanetPositionHelioGroup>
{
    public override string Name => Description.GetDescription(HarmonicTypeEnum.Harmonic120);
}