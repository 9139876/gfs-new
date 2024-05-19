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

    public override string Name => $"{(Direction > 0 ? "Восходящий" : "Нисходящий")} Угол {PriceStep}х{TimeStep}";

    private string Description { get; set; } = string.Empty;
    private bool IsPriceFaster { get; set; }

    public override ForecastCalculationResult Calculate(CalculationContext context)
    {
        var result = new ForecastCalculationResult();

        IsPriceFaster = PriceStep > TimeStep;

        var fasterStep = IsPriceFaster ? PriceStep : TimeStep;

        foreach (var point in context.PointsFrom)
        {
            var position = point;

            var priceTimePosition = context.GetPriceTimePosition(point);
            var startPointText = $"{priceTimePosition.Price.ToString(CultureInfo.InvariantCulture)} {priceTimePosition.Date.GetDateStringByTimeFrame(context.TimeFrame)}";
            Description = $"{(Direction > 0 ? "Восходящий" : "Нисходящий")} Угол {PriceStep}х{TimeStep} от {startPointText}";

            while (true)
            {
                if (IsPriceFaster)
                    position.X += 1;
                else
                    position.Y += Direction;

                for (var i = 0; i <= fasterStep; i++)
                {
                    if (IsPriceFaster)
                        position.Y += Direction;
                    else
                        position.X += 1;
                            
                    AddWithSpread(position, context, result);
                }

                if (!context.InSheet(position))
                    break;
            }
        }

        return result;
    }

    private void AddWithSpread(Point position, CalculationContext context, ForecastCalculationResult result)
    {
        for (var i = -context.ForecastSpread; i <= context.ForecastSpread; i++)
        {
            var positionWithSpread = IsPriceFaster
                ? new Point(position.X + i, position.Y)
                : new Point(position.X, position.Y + i);

            if (context.InSheet(positionWithSpread))
                result.AddForecastCalculationResultItem(new ForecastCalculationResultItem(
                    position: positionWithSpread,
                    descriptions: Description
                ));
        }
    }
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