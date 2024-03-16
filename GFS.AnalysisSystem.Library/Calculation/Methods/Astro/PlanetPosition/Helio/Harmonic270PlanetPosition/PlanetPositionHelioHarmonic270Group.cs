using GFS.AnalysisSystem.Library.Calculation.Abstraction;
using GFS.Common.Attributes;

namespace GFS.AnalysisSystem.Library.Calculation.Methods.Astro.PlanetPosition.Helio.Harmonic270PlanetPosition;

public class PlanetPositionHelioHarmonic270Group: ForecastTreeGroup<PlanetPositionHelioGroup>
{
    public override string Name => Description.GetDescription(HarmonicTypeEnum.Harmonic270);
}