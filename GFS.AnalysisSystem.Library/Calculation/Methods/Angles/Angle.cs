using System.Drawing;
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

            while (true)
            {
                position = new Point(position.X + TimeStep, position.Y + PriceStep * Direction);

                if (!context.InSheet(position))
                    break;

                //todo point price and time
                result.AddForecastCalculationResultItem(new ForecastCalculationResultItem(position, $"Угол {PriceStep}х{TimeStep} от [{point.X};{point.Y}]"));
            }
        }

        return result;
    }

    public override string Name => $"Угол {PriceStep}х{TimeStep} {(Direction > 0 ? "Восходящий" : "Нисходящий")}";
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