using GFS.AnalysisSystem.Library.Calculation.Abstraction;

namespace GFS.AnalysisSystem.Library.Calculation.Methods.Balances.VectorsBalances;

public class VectorsBalancesGroup: ForecastTreeGroup<BalancesGroup>
{
    public override string Name => "Балансы отрезков";
}