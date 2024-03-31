using System.Drawing;
using System.Globalization;
using GFS.AnalysisSystem.Library.Calculation.Abstraction;
using GFS.AnalysisSystem.Library.Calculation.Models;
using GFS.GrailCommon.Extensions;

namespace GFS.AnalysisSystem.Library.Calculation.Methods.Angles;

public abstract class Angle : ForecastTreeMethod<AnglesGroup>
{
    protected abstract int Direction { get; }
    protected abstract byte PriceStep { get; }
    protected abstract byte TimeStep { get; }

    public override ForecastCalculationResult Calculate(CalculationContext context)
    {
        var result = new ForecastCalculationResult();

        foreach (var point in context.PointsFrom)
        {
            var position = point;
            var priceTimePosition = context.GetPriceTimePosition(point);
            var positionText = $"{priceTimePosition.Price.ToString(CultureInfo.InvariantCulture)} {priceTimePosition.Date.GetDateStringByTimeFrame(context.TimeFrame)}";
            var results = new List<ForecastCalculationResultItem>();

            while (true)
            {
                for (var ts = 1; ts <= TimeStep; ts++)
                {
                    for (var ps = 1; ps <= PriceStep; ps++)
                    {
                        var stepPosition = new Point(position.X + ts, position.Y + ps * Direction);

                        for (var i = -context.ForecastSpread; i <= context.ForecastSpread; i++)
                        {
                            var positionWithSpread = new Point(stepPosition.X, stepPosition.Y + i);

                            if (context.InSheet(positionWithSpread))
                            {
                                results.Add(new ForecastCalculationResultItem(
                                    position: positionWithSpread,
                                    descriptions: $"{(Direction > 0 ? "Восходящий" : "Нисходящий")} Угол {PriceStep}х{TimeStep} от {positionText}")
                                );
                            }

                            positionWithSpread = new Point(stepPosition.X + i, stepPosition.Y);

                            if (context.InSheet(positionWithSpread))
                            {
                                results.Add(new ForecastCalculationResultItem(
                                    position: positionWithSpread,
                                    descriptions: $"{(Direction > 0 ? "Восходящий" : "Нисходящий")} Угол {PriceStep}х{TimeStep} от {positionText}")
                                );
                            }
                        }
                    }
                }

                position = new Point(position.X + TimeStep, position.Y + PriceStep * Direction);

                if (!context.InSheet(position))
                    break;
            }
            
            result.AddForecastCalculationResultItemList(results.Distinct(new ComparerForecastCalculationResultItemByPosition()));
        }

        return result;
    }

    public override string Name => $"{(Direction > 0 ? "Восходящий" : "Нисходящий")} Угол {PriceStep}х{TimeStep}";
}

public class Angle1X1Up : Angle
{
    protected override int Direction => 1;
    protected override byte PriceStep => 1;
    protected override byte TimeStep => 1;
}

public class Angle1X2Up : Angle
{
    protected override int Direction => 1;
    protected override byte PriceStep => 1;
    protected override byte TimeStep => 2;
}

public class Angle1X4Up : Angle
{
    protected override int Direction => 1;
    protected override byte PriceStep => 1;
    protected override byte TimeStep => 4;
}

public class Angle1X8Up : Angle
{
    protected override int Direction => 1;
    protected override byte PriceStep => 1;
    protected override byte TimeStep => 8;
}

public class Angle1X16Up : Angle
{
    protected override int Direction => 1;
    protected override byte PriceStep => 1;
    protected override byte TimeStep => 16;
}

public class Angle2X1Up : Angle
{
    protected override int Direction => 1;
    protected override byte PriceStep => 2;
    protected override byte TimeStep => 1;
}

public class Angle4X1Up : Angle
{
    protected override int Direction => 1;
    protected override byte PriceStep => 4;
    protected override byte TimeStep => 1;
}

public class Angle8X1Up : Angle
{
    protected override int Direction => 1;
    protected override byte PriceStep => 8;
    protected override byte TimeStep => 1;
}

public class Angle16X1Up : Angle
{
    protected override int Direction => 1;
    protected override byte PriceStep => 16;
    protected override byte TimeStep => 1;
}

public class Angle1X1Down : Angle
{
    protected override int Direction => -1;
    protected override byte PriceStep => 1;
    protected override byte TimeStep => 1;
}

public class Angle1X2Down : Angle
{
    protected override int Direction => -1;
    protected override byte PriceStep => 1;
    protected override byte TimeStep => 2;
}

public class Angle1X4Down : Angle
{
    protected override int Direction => -1;
    protected override byte PriceStep => 1;
    protected override byte TimeStep => 4;
}

public class Angle1X8Down : Angle
{
    protected override int Direction => -1;
    protected override byte PriceStep => 1;
    protected override byte TimeStep => 8;
}

public class Angle1X16Down : Angle
{
    protected override int Direction => -1;
    protected override byte PriceStep => 1;
    protected override byte TimeStep => 16;
}

public class Angle2X1Down : Angle
{
    protected override int Direction => -1;
    protected override byte PriceStep => 2;
    protected override byte TimeStep => 1;
}

public class Angle4X1Down : Angle
{
    protected override int Direction => -1;
    protected override byte PriceStep => 4;
    protected override byte TimeStep => 1;
}

public class Angle8X1Down : Angle
{
    protected override int Direction => -1;
    protected override byte PriceStep => 8;
    protected override byte TimeStep => 1;
}

public class Angle16X1Down : Angle
{
    protected override int Direction => -1;
    protected override byte PriceStep => 16;
    protected override byte TimeStep => 1;
}