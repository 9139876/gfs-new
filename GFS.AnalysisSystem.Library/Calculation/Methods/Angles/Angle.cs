using System.Drawing;
using System.Globalization;
using GFS.AnalysisSystem.Library.Calculation.Abstraction;
using GFS.AnalysisSystem.Library.Calculation.Models;

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
            var positionText = $"{priceTimePosition.Price.ToString(CultureInfo.InvariantCulture)} {priceTimePosition.Date}";

            while (true)
            {
                position = new Point(position.X + TimeStep, position.Y + PriceStep * Direction);

                for (var i = -context.ForecastSpread; i <= context.ForecastSpread; i++)
                {
                    var positionWithSpread = new Point(position.X, position.Y + i);

                    if (context.InSheet(positionWithSpread))
                    {
                        result.AddForecastCalculationResultItem(new ForecastCalculationResultItem(
                            position: positionWithSpread,
                            descriptions: $"{(Direction > 0 ? "Восходящий" : "Нисходящий")} Угол {PriceStep}х{TimeStep} от {positionText}")
                        );
                    }
                }

                if (!context.InSheet(position))
                    break;
            }
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