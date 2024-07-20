using System.Drawing;
using GFS.AnalysisSystem.Library.Calculation.Abstraction;
using GFS.AnalysisSystem.Library.Calculation.Models;
using GFS.Common.Extensions;

namespace GFS.AnalysisSystem.Library.Calculation.Methods.PriceLevels;

public abstract class GlobalPriceRange : ForecastTreeMethod<PriceLevelsGroup>
{
    public override ForecastCalculationResult Calculate(CalculationContext context)
    {
        var result = new ForecastCalculationResult();
        var maxPrice = context.Candles.Select(q => q.High).Max();
        var minPrice = context.Candles.Select(q => q.Low).Min();

        var range = maxPrice - minPrice;

        foreach (var i in Steps)
        {
            var priceCell = (int)Math.Round(minPrice + range * i);

            if (!context.ForecastWindow.PriceCellInWindow(priceCell))
                continue;

            var text = $"{(i * 100).ToHumanReadableNumber()}% глобального диапазона по цене";

            var point = new Point(context.ForecastWindow.Left, priceCell);

            while (context.ForecastWindow.InWindow(point))
            {
                for (var dy = -context.ForecastSpread; dy <= context.ForecastSpread; dy++)
                {
                    result.AddForecastCalculationResultItem(new ForecastCalculationResultItem(new Point(point.X, point.Y + dy), text));
                }

                point.X += 1;
            }
        }

        return result;
    }

    protected abstract decimal[] Steps { get; }
}

public class GlobalPriceRangeBy2 : GlobalPriceRange
{
    public override string Name => "Ценовые диапазоны /2";
    protected override decimal[] Steps => Enumerable.Range(0, 9).Select(x => x / 8m).ToArray();
}

public class GlobalPriceRangeBy3 : GlobalPriceRange
{
    public override string Name => "Ценовые диапазоны /3";
    protected override decimal[] Steps => Enumerable.Range(0, 7).Select(x => x / 6m).ToArray();
}