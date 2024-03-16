using GFS.AnalysisSystem.Library.Calculation.Abstraction;
using GFS.Common.Attributes;

namespace GFS.AnalysisSystem.Library.Calculation.Methods.Astro.PlanetPosition.Geo.ExactPlanetPosition;

public class PlanetPositionGeoExactGroup : ForecastTreeGroup<PlanetPositionGeoGroup>
{
    public override string Name => Description.GetDescription(HarmonicTypeEnum.Exact);
}