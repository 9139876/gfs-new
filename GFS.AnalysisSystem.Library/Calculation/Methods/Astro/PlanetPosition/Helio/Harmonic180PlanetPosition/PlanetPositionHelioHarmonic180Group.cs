using GFS.AnalysisSystem.Library.Calculation.Abstraction;
using GFS.Common.Attributes;

namespace GFS.AnalysisSystem.Library.Calculation.Methods.Astro.PlanetPosition.Helio.Harmonic180PlanetPosition;

public class PlanetPositionHelioHarmonic180Group: ForecastTreeGroup<PlanetPositionHelioGroup>
{
    public override string Name => Description.GetDescription(HarmonicTypeEnum.Harmonic180);
}