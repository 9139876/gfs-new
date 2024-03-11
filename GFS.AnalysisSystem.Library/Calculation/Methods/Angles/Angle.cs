using GFS.AnalysisSystem.Library.Calculation.Abstraction;
using GFS.AnalysisSystem.Library.Calculation.Models;

namespace GFS.AnalysisSystem.Library.Calculation.Methods.Angles;

public abstract class Angle : ForecastTreeMethod<AnglesGroup>
{
    
}

public class Angle1X1 : Angle
{
    public override ForecastCalculationResult Calculate(CalculationContext context)
    {
        throw new NotImplementedException();
    }

    public override string Name => "Угол 1х1";
}
