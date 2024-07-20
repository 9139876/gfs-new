using GFS.AnalysisSystem.Library.Calculation.Abstraction;

namespace GFS.AnalysisSystem.Library.Calculation.Methods.Balances.PointsBalances;

public class PointsBalancesGroup : ForecastTreeGroup<BalancesGroup>
{
    public override string Name => "Балансы точек";
}