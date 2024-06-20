using System;
using GFS.AnalysisSystem.Library.Internal.WheelCalculator;
using GFS.AnalysisSystem.Library.Internal.WheelCalculator.SquareOfNine;
using Xunit;

namespace UnitTests.AnalysisSystemTests;

public class SquareOfNineTests
{
    [Fact]
    public void Smoke_Test()
    {
        for (ushort i = 1; i <= 1000; i++)
            WheelCalculator<SquareOfNineWheel>.GetNumberAngle(i);
    }

    [Fact]
    public void GetNumberAngle_Test()
    {
        Assert.Equal(0, WheelCalculator<SquareOfNineWheel>.GetNumberAngle(1));
        Assert.Equal(0, WheelCalculator<SquareOfNineWheel>.GetNumberAngle(2));
        Assert.Equal(45, WheelCalculator<SquareOfNineWheel>.GetNumberAngle(3));
        Assert.Equal(90, WheelCalculator<SquareOfNineWheel>.GetNumberAngle(4));
        Assert.Equal(135, WheelCalculator<SquareOfNineWheel>.GetNumberAngle(5));
        Assert.Equal(180, WheelCalculator<SquareOfNineWheel>.GetNumberAngle(6));
        Assert.Equal(225, WheelCalculator<SquareOfNineWheel>.GetNumberAngle(7));
        Assert.Equal(270, WheelCalculator<SquareOfNineWheel>.GetNumberAngle(8));
        Assert.Equal(315, WheelCalculator<SquareOfNineWheel>.GetNumberAngle(9));

        Assert.Equal((decimal)Math.Round(Math.Atan((double)0 / 9) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(298), 3));
        Assert.Equal((decimal)Math.Round(Math.Atan((double)1 / 9) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(299), 3));
        Assert.Equal((decimal)Math.Round(Math.Atan((double)2 / 9) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(300), 3));
        Assert.Equal((decimal)Math.Round(Math.Atan((double)3 / 9) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(301), 3));
        Assert.Equal((decimal)Math.Round(Math.Atan((double)4 / 9) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(302), 3));
        Assert.Equal((decimal)Math.Round(Math.Atan((double)5 / 9) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(303), 3));
        Assert.Equal((decimal)Math.Round(Math.Atan((double)6 / 9) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(304), 3));
        Assert.Equal((decimal)Math.Round(Math.Atan((double)7 / 9) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(305), 3));
        Assert.Equal((decimal)Math.Round(Math.Atan((double)8 / 9) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(306), 3));

        Assert.Equal((decimal)Math.Round(Math.Atan((double)9 / 9) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(307), 3));
        Assert.Equal((decimal)Math.Round(Math.Atan((double)9 / 8) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(308), 3));
        Assert.Equal((decimal)Math.Round(Math.Atan((double)9 / 7) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(309), 3));
        Assert.Equal((decimal)Math.Round(Math.Atan((double)9 / 6) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(310), 3));
        Assert.Equal((decimal)Math.Round(Math.Atan((double)9 / 5) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(311), 3));
        Assert.Equal((decimal)Math.Round(Math.Atan((double)9 / 4) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(312), 3));
        Assert.Equal((decimal)Math.Round(Math.Atan((double)9 / 3) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(313), 3));
        Assert.Equal((decimal)Math.Round(Math.Atan((double)9 / 2) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(314), 3));
        Assert.Equal((decimal)Math.Round(Math.Atan((double)9 / 1) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(315), 3));

        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)9 / -1) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(317), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)9 / -2) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(318), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)9 / -3) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(319), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)9 / -4) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(320), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)9 / -5) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(321), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)9 / -6) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(322), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)9 / -7) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(323), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)9 / -8) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(324), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)9 / -9) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(325), 3));

        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)8 / -9) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(326), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)7 / -9) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(327), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)6 / -9) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(328), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)5 / -9) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(329), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)4 / -9) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(330), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)3 / -9) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(331), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)2 / -9) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(332), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)1 / -9) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(333), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)0 / -9) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(334), 3));

        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)-1 / -9) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(335), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)-2 / -9) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(336), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)-3 / -9) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(337), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)-4 / -9) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(338), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)-5 / -9) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(339), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)-6 / -9) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(340), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)-7 / -9) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(341), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)-8 / -9) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(342), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)-9 / -9) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(343), 3));

        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)-9 / -8) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(344), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)-9 / -7) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(345), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)-9 / -6) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(346), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)-9 / -5) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(347), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)-9 / -4) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(348), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)-9 / -3) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(349), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)-9 / -2) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(350), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)-9 / -1) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(351), 3));

        Assert.Equal(360 + (decimal)Math.Round(Math.Atan((double)-9 / 1) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(353), 3));
        Assert.Equal(360 + (decimal)Math.Round(Math.Atan((double)-9 / 2) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(354), 3));
        Assert.Equal(360 + (decimal)Math.Round(Math.Atan((double)-9 / 3) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(355), 3));
        Assert.Equal(360 + (decimal)Math.Round(Math.Atan((double)-9 / 4) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(356), 3));
        Assert.Equal(360 + (decimal)Math.Round(Math.Atan((double)-9 / 5) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(357), 3));
        Assert.Equal(360 + (decimal)Math.Round(Math.Atan((double)-9 / 6) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(358), 3));
        Assert.Equal(360 + (decimal)Math.Round(Math.Atan((double)-9 / 7) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(359), 3));
        Assert.Equal(360 + (decimal)Math.Round(Math.Atan((double)-9 / 8) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(360), 3));
        Assert.Equal(360 + (decimal)Math.Round(Math.Atan((double)-9 / 9) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(361), 3));

        Assert.Equal(360 + (decimal)Math.Round(Math.Atan((double)-8 / 9) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(290), 3));
        Assert.Equal(360 + (decimal)Math.Round(Math.Atan((double)-7 / 9) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(291), 3));
        Assert.Equal(360 + (decimal)Math.Round(Math.Atan((double)-6 / 9) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(292), 3));
        Assert.Equal(360 + (decimal)Math.Round(Math.Atan((double)-5 / 9) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(293), 3));
        Assert.Equal(360 + (decimal)Math.Round(Math.Atan((double)-4 / 9) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(294), 3));
        Assert.Equal(360 + (decimal)Math.Round(Math.Atan((double)-3 / 9) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(295), 3));
        Assert.Equal(360 + (decimal)Math.Round(Math.Atan((double)-2 / 9) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(296), 3));
        Assert.Equal(360 + (decimal)Math.Round(Math.Atan((double)-1 / 9) * 360 / (2 * Math.PI), 3), Math.Round(WheelCalculator<SquareOfNineWheel>.GetNumberAngle(297), 3));
    }

    [Fact]
    public void GetDegreesBetweenNumbers_Test()
    {
        Assert.Equal(0, WheelCalculator<SquareOfNineWheel>.GetDegreesBetweenNumbers(9, 25));
        Assert.Equal(90, WheelCalculator<SquareOfNineWheel>.GetDegreesBetweenNumbers(86, 96));
        Assert.Equal(90, WheelCalculator<SquareOfNineWheel>.GetDegreesBetweenNumbers(96, 86));
        Assert.Equal(90, WheelCalculator<SquareOfNineWheel>.GetDegreesBetweenNumbers(91, 101));
        Assert.Equal(0, WheelCalculator<SquareOfNineWheel>.GetDegreesBetweenNumbers(86, 176));
        Assert.Equal(0, WheelCalculator<SquareOfNineWheel>.GetDegreesBetweenNumbers(176, 86));

        Assert.Equal(0.744m, Math.Round(WheelCalculator<SquareOfNineWheel>.GetDegreesBetweenNumbers(5930, 5929), 3));
        Assert.Equal(0.744m, Math.Round(WheelCalculator<SquareOfNineWheel>.GetDegreesBetweenNumbers(5929, 5930), 3));
    }

    [Fact]
    public void GetFullDegreesBetweenNumbers_Test()
    {
        Assert.Equal(360, WheelCalculator<SquareOfNineWheel>.GetFullDegreesBetweenNumbers(9, 25));
        Assert.Equal(90, WheelCalculator<SquareOfNineWheel>.GetFullDegreesBetweenNumbers(86, 96));
        Assert.Equal(90, WheelCalculator<SquareOfNineWheel>.GetFullDegreesBetweenNumbers(96, 86));
        Assert.Equal(90, WheelCalculator<SquareOfNineWheel>.GetFullDegreesBetweenNumbers(91, 101));
        Assert.Equal(720, WheelCalculator<SquareOfNineWheel>.GetFullDegreesBetweenNumbers(86, 176));
        Assert.Equal(720, WheelCalculator<SquareOfNineWheel>.GetFullDegreesBetweenNumbers(176, 86));

        Assert.Equal(0.744m, Math.Round(WheelCalculator<SquareOfNineWheel>.GetFullDegreesBetweenNumbers(5930, 5929), 3));
        Assert.Equal(0.744m, Math.Round(WheelCalculator<SquareOfNineWheel>.GetFullDegreesBetweenNumbers(5929, 5930), 3));
    }
}