using GFS.AnalysisSystem.Library.Calculation.Abstraction;
using GFS.AnalysisSystem.Library.Calculation.Models;
using GFS.AnalysisSystem.Library.Internal.TimeConverter;
using GFS.Common.Extensions;
using GFS.GrailCommon.Extensions;
using GFS.GrailCommon.Models;

namespace GFS.AnalysisSystem.Library.Calculation.Methods.Balances.VectorsBalances;

public class VectorTimeSpanToTimeBalance : ForecastTreeMethod<VectorsBalancesGroup>
{
    public override ForecastCalculationResult Calculate(CalculationContext context)
    {
        Func<PriceTimePoint, string> getText = point => $"{point.Price.ToHumanReadableNumber()} {point.Date.GetDateStringByTimeFrame(context.TimeFrame)}";

        var targetPointPriceTimePosition = context.GetPriceTimePosition(context.TargetPoint);
        var rangeLeft = TimeConverter.GetTimeSpan(Math.Max(context.ForecastWindow.Left - context.TargetPoint.X, 5), context.TimeFrame);
        var rangeRight = TimeConverter.GetTimeSpan(Math.Max(context.ForecastWindow.Right - context.TargetPoint.X, 5), context.TimeFrame);
        var range = new TimeRange(rangeLeft, rangeRight);
        var targetPointText = getText(targetPointPriceTimePosition);

        var result = new ForecastCalculationResult();

        foreach (var sourcePoint in context.SourcePoints)
        {
            var sourcePointPriceTimePosition = context.GetPriceTimePosition(sourcePoint);
            var vectorTimeSpan = targetPointPriceTimePosition.Date - sourcePointPriceTimePosition.Date;
            var sourcePointText = getText(sourcePointPriceTimePosition);
            var vectorText = $"[{sourcePointText} - {targetPointText}]";

            foreach (var unitOfTimeValue in TimeConverter.TimeSpanToUnitOfTimeValues(vectorTimeSpan))
            {
                foreach (var resultItem in TimeConverter.ConvertNumber(unitOfTimeValue.Value, range))
                {
                    var timeValue = context.DateToCell(targetPointPriceTimePosition.Date.Add(resultItem.Value));
                    var description = $"Баланс продолжительности отрезка {vectorText} {unitOfTimeValue.Description} и {resultItem.Description}";

                    context.AddTimeValueWithSpread(timeValue, description, result);
                }
            }
        }

        return result;
    }

    public override string Name => "Баланс длительности отрезка со временем";
}