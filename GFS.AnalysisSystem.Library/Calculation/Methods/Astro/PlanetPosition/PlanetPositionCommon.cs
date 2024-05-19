using System.Drawing;
using GFS.AnalysisSystem.Library.Calculation.Models;
using GFS.AnalysisSystem.Library.Internal.AstroCommon;
using GFS.Common.Attributes;
using GFS.GrailCommon.Extensions;

namespace GFS.AnalysisSystem.Library.Calculation.Methods.Astro.PlanetPosition;

public static class PlanetPositionCommon
{
    public static ForecastCalculationResult Calculate(CalculationContext context, Planet planet, HarmonicTypeEnum harmonicType)
    {
        var description = $"{Description.GetDescription(harmonicType)} {planet.Name}";
        var result = new ForecastCalculationResult();

        for (var x = 0; x < context.SheetSizeInCells.Width; x++)
        {
            var date = context.TimeValues[x];
            var position = EphemCalculation.CalcPlanetLongitude(date, planet);

            if (!position.IsSuccess)
                throw new InvalidOperationException($"Ошибка вычисления положения планеты {planet.Name} на дату {date.GetDateStringByTimeFrame(context.TimeFrame)}");

            var longitude = (int)Math.Round(EphemCalculation.GetHarmonicLongitude(position.Result, harmonicType));

            for (var i = 0; i < int.MaxValue; i++)
            {
                var point = new Point(x, longitude + 360 * i);

                var pointsWithSpread = GetPointsWithSpread(point, context);

                if (pointsWithSpread.Any())
                    result.AddForecastCalculationResultItemList(pointsWithSpread.Select(p => new ForecastCalculationResultItem(p, description)));
                else
                    break;
            }
        }

        return result;
    }

    private static List<Point> GetPointsWithSpread(Point position, CalculationContext context)
    {
        var items = new List<Point>();

        for (var i = -context.ForecastSpread; i <= context.ForecastSpread; i++)
        {
            var positionWithSpread = new Point(position.X, position.Y + i);

            if (context.InSheet(positionWithSpread))
                items.Add(positionWithSpread);
        }

        return items;
    }
}