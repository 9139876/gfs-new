using System;
using GFS.AnalysisSystem.Library.Internal.SquareOfNine;
using Xunit;

namespace UnitTests.AnalysisSystemTests;

public class SquareOfNineTests
{
    [Fact]
    public void Smoke_Test()
    {
        for (ushort i = 1; i <= 1000; i++)
            SquareOfNine.GetNumberAngle(i);
    }

    [Fact]
    public void GetNumberAngle_Test()
    {
        Assert.Equal(0, SquareOfNine.GetNumberAngle(1));
        Assert.Equal(0, SquareOfNine.GetNumberAngle(2));
        Assert.Equal(45, SquareOfNine.GetNumberAngle(3));
        Assert.Equal(90, SquareOfNine.GetNumberAngle(4));
        Assert.Equal(135, SquareOfNine.GetNumberAngle(5));
        Assert.Equal(180, SquareOfNine.GetNumberAngle(6));
        Assert.Equal(225, SquareOfNine.GetNumberAngle(7));
        Assert.Equal(270, SquareOfNine.GetNumberAngle(8));
        Assert.Equal(315, SquareOfNine.GetNumberAngle(9));

        Assert.Equal((decimal)Math.Round(Math.Atan((double)0 / 9) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(298), 3));
        Assert.Equal((decimal)Math.Round(Math.Atan((double)1 / 9) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(299), 3));
        Assert.Equal((decimal)Math.Round(Math.Atan((double)2 / 9) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(300), 3));
        Assert.Equal((decimal)Math.Round(Math.Atan((double)3 / 9) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(301), 3));
        Assert.Equal((decimal)Math.Round(Math.Atan((double)4 / 9) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(302), 3));
        Assert.Equal((decimal)Math.Round(Math.Atan((double)5 / 9) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(303), 3));
        Assert.Equal((decimal)Math.Round(Math.Atan((double)6 / 9) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(304), 3));
        Assert.Equal((decimal)Math.Round(Math.Atan((double)7 / 9) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(305), 3));
        Assert.Equal((decimal)Math.Round(Math.Atan((double)8 / 9) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(306), 3));

        Assert.Equal((decimal)Math.Round(Math.Atan((double)9 / 9) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(307), 3));
        Assert.Equal((decimal)Math.Round(Math.Atan((double)9 / 8) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(308), 3));
        Assert.Equal((decimal)Math.Round(Math.Atan((double)9 / 7) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(309), 3));
        Assert.Equal((decimal)Math.Round(Math.Atan((double)9 / 6) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(310), 3));
        Assert.Equal((decimal)Math.Round(Math.Atan((double)9 / 5) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(311), 3));
        Assert.Equal((decimal)Math.Round(Math.Atan((double)9 / 4) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(312), 3));
        Assert.Equal((decimal)Math.Round(Math.Atan((double)9 / 3) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(313), 3));
        Assert.Equal((decimal)Math.Round(Math.Atan((double)9 / 2) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(314), 3));
        Assert.Equal((decimal)Math.Round(Math.Atan((double)9 / 1) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(315), 3));

        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)9 / -1) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(317), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)9 / -2) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(318), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)9 / -3) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(319), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)9 / -4) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(320), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)9 / -5) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(321), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)9 / -6) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(322), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)9 / -7) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(323), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)9 / -8) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(324), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)9 / -9) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(325), 3));

        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)8 / -9) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(326), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)7 / -9) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(327), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)6 / -9) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(328), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)5 / -9) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(329), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)4 / -9) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(330), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)3 / -9) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(331), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)2 / -9) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(332), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)1 / -9) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(333), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)0 / -9) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(334), 3));

        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)-1 / -9) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(335), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)-2 / -9) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(336), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)-3 / -9) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(337), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)-4 / -9) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(338), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)-5 / -9) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(339), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)-6 / -9) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(340), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)-7 / -9) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(341), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)-8 / -9) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(342), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)-9 / -9) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(343), 3));

        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)-9 / -8) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(344), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)-9 / -7) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(345), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)-9 / -6) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(346), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)-9 / -5) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(347), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)-9 / -4) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(348), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)-9 / -3) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(349), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)-9 / -2) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(350), 3));
        Assert.Equal(180 + (decimal)Math.Round(Math.Atan((double)-9 / -1) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(351), 3));

        Assert.Equal(360 + (decimal)Math.Round(Math.Atan((double)-9 / 1) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(353), 3));
        Assert.Equal(360 + (decimal)Math.Round(Math.Atan((double)-9 / 2) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(354), 3));
        Assert.Equal(360 + (decimal)Math.Round(Math.Atan((double)-9 / 3) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(355), 3));
        Assert.Equal(360 + (decimal)Math.Round(Math.Atan((double)-9 / 4) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(356), 3));
        Assert.Equal(360 + (decimal)Math.Round(Math.Atan((double)-9 / 5) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(357), 3));
        Assert.Equal(360 + (decimal)Math.Round(Math.Atan((double)-9 / 6) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(358), 3));
        Assert.Equal(360 + (decimal)Math.Round(Math.Atan((double)-9 / 7) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(359), 3));
        Assert.Equal(360 + (decimal)Math.Round(Math.Atan((double)-9 / 8) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(360), 3));
        Assert.Equal(360 + (decimal)Math.Round(Math.Atan((double)-9 / 9) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(361), 3));

        Assert.Equal((decimal)Math.Round(Math.Atan((double)-8 / 9) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(290), 3));
        Assert.Equal((decimal)Math.Round(Math.Atan((double)-7 / 9) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(291), 3));
        Assert.Equal((decimal)Math.Round(Math.Atan((double)-6 / 9) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(292), 3));
        Assert.Equal((decimal)Math.Round(Math.Atan((double)-5 / 9) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(293), 3));
        Assert.Equal((decimal)Math.Round(Math.Atan((double)-4 / 9) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(294), 3));
        Assert.Equal((decimal)Math.Round(Math.Atan((double)-3 / 9) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(295), 3));
        Assert.Equal((decimal)Math.Round(Math.Atan((double)-2 / 9) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(296), 3));
        Assert.Equal((decimal)Math.Round(Math.Atan((double)-1 / 9) * 360 / (2 * Math.PI), 3), Math.Round(SquareOfNine.GetNumberAngle(297), 3));
    }

    [Fact]
    public void GetDegreesBetweenNumbers_Test()
    {
        Assert.Equal(0, SquareOfNine.GetDegreesBetweenNumbers(9, 25));
        Assert.Equal(90, SquareOfNine.GetDegreesBetweenNumbers(86, 96));
        Assert.Equal(90, SquareOfNine.GetDegreesBetweenNumbers(96, 86));
        Assert.Equal(90, SquareOfNine.GetDegreesBetweenNumbers(91, 101));
    }

    [Fact]
    public void GetFullDegreesBetweenNumbers_Test()
    {
        Assert.Equal(360, SquareOfNine.GetFullDegreesBetweenNumbers(9, 25));
        Assert.Equal(90, SquareOfNine.GetFullDegreesBetweenNumbers(86, 96));
        Assert.Equal(90, SquareOfNine.GetFullDegreesBetweenNumbers(96, 86));
        Assert.Equal(90, SquareOfNine.GetFullDegreesBetweenNumbers(91, 101));
        Assert.Equal(720, SquareOfNine.GetFullDegreesBetweenNumbers(86, 176));
        Assert.Equal(720, SquareOfNine.GetFullDegreesBetweenNumbers(176, 86));
    }
}