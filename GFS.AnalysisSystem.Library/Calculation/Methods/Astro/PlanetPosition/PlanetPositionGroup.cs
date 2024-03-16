using GFS.AnalysisSystem.Library.Calculation.Abstraction;

namespace GFS.AnalysisSystem.Library.Calculation.Methods.Astro.PlanetPosition;

public class PlanetPositionGroup : ForecastTreeGroup<AstroGroup>
{
    public override string Name => "Положение планет";
}