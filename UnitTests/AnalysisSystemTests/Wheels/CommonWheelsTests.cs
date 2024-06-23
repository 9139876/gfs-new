using GFS.AnalysisSystem.Library.Internal.WheelCalculator;
using GFS.AnalysisSystem.Library.Internal.WheelCalculator.CircleOfNine;
using GFS.AnalysisSystem.Library.Internal.WheelCalculator.Hexagon;
using GFS.AnalysisSystem.Library.Internal.WheelCalculator.SquareOfNine;
using Xunit;

namespace UnitTests.AnalysisSystemTests.Wheels;

public class CommonWheelsTests
{
    [Fact]
    public void SmokeMix_Test()
    {
        for (ushort i = 1; i <= 1000; i++)
        {
            WheelCalculator<SquareOfNineWheel>.GetNumberAngle(i);
            WheelCalculator<CircleOfNineWheel>.GetNumberAngle(i);
            WheelCalculator<HexagonWheel>.GetNumberAngle(i);
        }
            
    }
}