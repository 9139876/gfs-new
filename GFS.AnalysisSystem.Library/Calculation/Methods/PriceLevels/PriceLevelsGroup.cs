using GFS.AnalysisSystem.Library.Calculation.Abstraction;

namespace GFS.AnalysisSystem.Library.Calculation.Methods.PriceLevels;

public class PriceLevelsGroup : ForecastTreeGroup<ForecastTreeRoot>
{
    public override string Name => "Уровни по цене";
}