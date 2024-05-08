using GFS.AnalysisSystem.Library.Calculation.Abstraction;

namespace GFS.AnalysisSystem.Library.Calculation.Methods.Astro.PlanetPosition.Geo;

public class PlanetPositionGeoGroup : ForecastTreeGroup<PlanetPositionGroup>
{
    public override string Name => "Гео";
}