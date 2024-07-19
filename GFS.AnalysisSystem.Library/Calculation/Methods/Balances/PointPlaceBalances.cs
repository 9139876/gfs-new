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
    protected void AddTimeValueWithSpread(int timeInCells, string description, CalculationContext context, ForecastCalculationResult result)
    {
        for (var spread = -context.ForecastSpread; spread <= context.ForecastSpread; spread++)
        {
            for (var y = context.ForecastWindow.Bottom; y <= context.ForecastWindow.Top; y++)
            {
                result.AddForecastCalculationResultItem(new ForecastCalculationResultItem(new Point(timeInCells + spread, y), description));
            }
        }
    }

    protected void AddPriceValueWithSpread(int priceInCells, string description, CalculationContext context, ForecastCalculationResult result)
    {
        for (var spread = -context.ForecastSpread; spread <= context.ForecastSpread; spread++)
        {
            for (var x = context.ForecastWindow.Left; x <= context.ForecastWindow.Right; x++)
            {
                result.AddForecastCalculationResultItem(new ForecastCalculationResultItem(new Point(x, priceInCells + spread), description));
            }
        }
    }
}

public class AbsPricePointBalance : PointPlaceBalances
{
    public override ForecastCalculationResult Calculate(CalculationContext context)
    {
        return CalculateFromPoint(context, context.TargetPoint);
    }

    private ForecastCalculationResult CalculateFromPoint(CalculationContext context, Point point)
    {
        throw new NotImplementedException();

        // var result = new ForecastCalculationResult();
        // var priceTimePosition = context.GetPriceTimePosition(point);
        // var startPointText = $"{priceTimePosition.Price.ToString(CultureInfo.InvariantCulture)} {priceTimePosition.Date.GetDateStringByTimeFrame(context.TimeFrame)}";
        // var range = new TimeRange((ushort)Math.Max(context.ForecastWindow.Left - point.X, 5), (ushort)Math.Max(context.ForecastWindow.Right - point.X, 0));
        //
        // var timeValues = TimeConverter.Convert(priceTimePosition.Price, context.TimeFrame, range);
        //
        // foreach (var value in timeValues)
        // {
        //     var timeValue = context.DateToCell(priceTimePosition.Date.AddDate(context.TimeFrame, value.Value));
        //     var description = $"Баланс от {startPointText} - значение цены {priceTimePosition.Price.ToString(CultureInfo.InvariantCulture)} и {value.Description}";
        //
        //     AddTimeValueWithSpread(timeValue, description, context, result);
        // }
        //
        // return result;
    }

    public override string Name => "Баланс от абсолютного значения цены";
}