using GFS.AnalysisSystem.Library.Internal.WheelCalculator;
using GFS.AnalysisSystem.Library.Internal.WheelCalculator.Hexagon;
using Xunit;

namespace UnitTests.AnalysisSystemTests.Wheels;

public class HexagonTests
{
    [Fact]
    public void Smoke_Test()
    {
        for (ushort i = 1; i <= 1000; i++)
            WheelCalculator<HexagonWheel>.GetNumberAngle(i);
    }

    [Fact]
    public void GetNumberAngle_Test()
    {
        Assert.Equal(60, WheelCalculator<HexagonWheel>.GetNumberAngle(1));
        Assert.Equal(120, WheelCalculator<HexagonWheel>.GetNumberAngle(2));
        Assert.Equal(180, WheelCalculator<HexagonWheel>.GetNumberAngle(3));
        Assert.Equal(240, WheelCalculator<HexagonWheel>.GetNumberAngle(4));
        Assert.Equal(300, WheelCalculator<HexagonWheel>.GetNumberAngle(5));
        Assert.Equal(0, WheelCalculator<HexagonWheel>.GetNumberAngle(6));

        Assert.Equal(30, WheelCalculator<HexagonWheel>.GetNumberAngle(7));
        Assert.Equal(60, WheelCalculator<HexagonWheel>.GetNumberAngle(8));
        Assert.Equal(90, WheelCalculator<HexagonWheel>.GetNumberAngle(9));
        Assert.Equal(120, WheelCalculator<HexagonWheel>.GetNumberAngle(10));
        Assert.Equal(150, WheelCalculator<HexagonWheel>.GetNumberAngle(11));
        Assert.Equal(180, WheelCalculator<HexagonWheel>.GetNumberAngle(12));
        Assert.Equal(210, WheelCalculator<HexagonWheel>.GetNumberAngle(13));
        Assert.Equal(240, WheelCalculator<HexagonWheel>.GetNumberAngle(14));
        Assert.Equal(270, WheelCalculator<HexagonWheel>.GetNumberAngle(15));
        Assert.Equal(300, WheelCalculator<HexagonWheel>.GetNumberAngle(16));
        Assert.Equal(330, WheelCalculator<HexagonWheel>.GetNumberAngle(17));
        Assert.Equal(0, WheelCalculator<HexagonWheel>.GetNumberAngle(18));
        
        Assert.Equal(60, WheelCalculator<HexagonWheel>.GetNumberAngle(40));
        Assert.Equal(60, WheelCalculator<HexagonWheel>.GetNumberAngle(1045));

        Assert.Equal(240, WheelCalculator<HexagonWheel>.GetNumberAngle(200));

        Assert.Equal(270, WheelCalculator<HexagonWheel>.GetNumberAngle(315));
    }
}