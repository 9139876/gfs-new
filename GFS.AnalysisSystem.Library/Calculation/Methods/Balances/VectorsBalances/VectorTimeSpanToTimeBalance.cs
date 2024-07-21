using GFS.AnalysisSystem.Library.Calculation.Abstraction;
using GFS.AnalysisSystem.Library.Calculation.Models;
using GFS.AnalysisSystem.Library.Calculation.Models.Internal;
using GFS.AnalysisSystem.Library.Internal.TimeConverter;

namespace GFS.AnalysisSystem.Library.Calculation.Methods.Balances.VectorsBalances;

public class VectorTimeSpanToTimeBalance : ForecastTreeMethod<VectorsBalancesGroup>
{
    public override ForecastCalculationResult Calculate(CalculationContext context)
    {
        var result = new ForecastCalculationResult();

        foreach (var vector in context.SourcePoints.Select(sp => new PriceTimeVector(sp, context.TargetPoint, context)))
        {
            foreach (var (fromBalance, toBalance) in TimeConverter.ConvertTimeSpan(vector.TimeSpan, context.GetForecastTimeRange()))
            {
                var timeValue = context.DateToCell(vector.TargetPoint.Date.Add(toBalance.TimeSpanValue));
                var description = $"Баланс продолжительности отрезка {vector.Name} {fromBalance.Description} и {toBalance.Description}";

                context.AddTimeValueWithSpread(timeValue, description, result);
            }
        }

        return result;
    }

    public override string Name => "Баланс длительности отрезка со временем";
}