using GFS.AnalysisSystem.Library.Internal.WheelCalculator;
using GFS.AnalysisSystem.Library.Internal.WheelCalculator.CircleOfNine;
using Xunit;

namespace UnitTests.AnalysisSystemTests.Wheels;

public class CircleOfNineTests
{
    [Fact]
    public void Smoke_Test()
    {
        for (ushort i = 1; i <= 1000; i++)
            WheelCalculator<CircleOfNineWheel>.GetNumberAngle(i);
    }

    [Fact]
    public void GetNumberAngle_Test()
    {
        Assert.Equal(0, WheelCalculator<CircleOfNineWheel>.GetNumberAngle(1));
        Assert.Equal(22.5m, WheelCalculator<CircleOfNineWheel>.GetNumberAngle(2));
        Assert.Equal(67.5m, WheelCalculator<CircleOfNineWheel>.GetNumberAngle(3));
        Assert.Equal(112.5m, WheelCalculator<CircleOfNineWheel>.GetNumberAngle(4));
        Assert.Equal(157.5m, WheelCalculator<CircleOfNineWheel>.GetNumberAngle(5));
        Assert.Equal(202.5m, WheelCalculator<CircleOfNineWheel>.GetNumberAngle(6));
        Assert.Equal(247.5m, WheelCalculator<CircleOfNineWheel>.GetNumberAngle(7));
        Assert.Equal(292.5m, WheelCalculator<CircleOfNineWheel>.GetNumberAngle(8));
        Assert.Equal(337.5m, WheelCalculator<CircleOfNineWheel>.GetNumberAngle(9));
    }
}