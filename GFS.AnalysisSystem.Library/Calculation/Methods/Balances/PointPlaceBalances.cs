using System.Drawing;
using System.Globalization;
using GFS.AnalysisSystem.Library.Calculation.Abstraction;
using GFS.AnalysisSystem.Library.Calculation.Models;
using GFS.AnalysisSystem.Library.Internal.TimeConverter;
using GFS.GrailCommon.Extensions;

namespace GFS.AnalysisSystem.Library.Calculation.Methods.Balances;

/// <summary>
/// Балансы относительно места предыдущей точки
/// </summary>
public abstract class PointPlaceBalances : ForecastTreeMethod<BalancesGroup>
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
        var startPointText = $"{priceTimePosition.Price.ToString(CultureInfo.InvariantCulture)} {priceTimePosition.Date.GetDateStringByTimeFrame(context.TimeFrame)}";
        
        var rangeLeft = TimeConverter.GetTimeSpan(Math.Max(context.ForecastWindow.Left - point.X, 5), context.TimeFrame);
        var rangeRight = TimeConverter.GetTimeSpan(Math.Max(context.ForecastWindow.Right - point.X, 5), context.TimeFrame);
        var range = new TimeRange(rangeLeft,rangeRight);
        
        var timeValues = TimeConverter.ConvertNumber(priceTimePosition.Price, range);
        
        foreach (var value in timeValues)
        {
            var timeValue = context.DateToCell(priceTimePosition.Date.Add(value.Value));
            var description = $"Баланс от {startPointText} - значение цены {priceTimePosition.Price.ToString(CultureInfo.InvariantCulture)} и {value.Description}";
        
            context.AddTimeValueWithSpread(timeValue, description, result);
        }

        return result;
    }

    public override string Name => "Баланс от абсолютного значения цены";
}