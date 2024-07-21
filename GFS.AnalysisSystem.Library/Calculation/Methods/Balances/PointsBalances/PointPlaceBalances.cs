using System.Drawing;
using GFS.AnalysisSystem.Library.Calculation.Abstraction;
using GFS.AnalysisSystem.Library.Calculation.Models;
using GFS.AnalysisSystem.Library.Internal.TimeConverter;
using GFS.Common.Extensions;
using GFS.GrailCommon.Extensions;

namespace GFS.AnalysisSystem.Library.Calculation.Methods.Balances.PointsBalances;

/// <summary>
/// Балансы относительно места предыдущей точки
/// </summary>
public abstract class PointPlaceBalances : ForecastTreeMethod<PointsBalancesGroup>
{
}

public class AbsPricePointBalance : PointPlaceBalances
{
    public override ForecastCalculationResult Calculate(CalculationContext context)
    {
        return CalculateFromPoint(context, context.TargetPoint);
    }

    private ForecastCalculationResult CalculateFromPoint(CalculationContext context, Point point)
    {
        var result = new ForecastCalculationResult();

        var priceTimePosition = context.GetPriceTimePosition(point);
        var startPointText = $"{priceTimePosition.Price.ToHumanReadableNumber()} {priceTimePosition.Date.GetDateStringByTimeFrame(context.TimeFrame)}";

        var timeValues = TimeConverter.ConvertNumber(priceTimePosition.Price, context.GetForecastTimeRange());

        foreach (var item in timeValues)
        {
            var timeValue = context.DateToCell(priceTimePosition.Date.Add(item.TimeSpanValue));
            var description = $"Баланс от {startPointText} - значение цены {priceTimePosition.Price.ToHumanReadableNumber()} и {item.Description}";

            context.AddTimeValueWithSpread(timeValue, description, result);
        }

        return result;
    }

    public override string Name => "Баланс от абсолютного значения цены";
}