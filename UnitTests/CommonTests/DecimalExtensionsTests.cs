using GFS.Common.Extensions;
using Xunit;

namespace UnitTests.CommonTests;

public class DecimalExtensionsTests
{
    [Fact]
    public void ToThreeDigitNumber_Test()
    {
        Assert.Equal(300m.ToThreeDigitNumber(), 300m);
        Assert.Equal(100m.ToThreeDigitNumber(), 100m);
        Assert.Equal(999m.ToThreeDigitNumber(), 999m);
        Assert.Equal(1000m.ToThreeDigitNumber(), 100m);
        Assert.Equal(10000m.ToThreeDigitNumber(), 100m);
        Assert.Equal(9999999m.ToThreeDigitNumber(), 100m);
        Assert.Equal(9991m.ToThreeDigitNumber(), 999m);
        Assert.Equal(10m.ToThreeDigitNumber(), 100m);
        Assert.Equal(1m.ToThreeDigitNumber(), 100m);
        Assert.Equal(0.00001m.ToThreeDigitNumber(), 100m);
        Assert.Equal(0.999999m.ToThreeDigitNumber(), 100m);
        Assert.Equal(0.9991m.ToThreeDigitNumber(), 999m);
    }

    [Fact]
    public void ToHumanReadableNumber_Test()
    {
        Assert.Equal(2000.123456789m.ToHumanReadableNumber(), "2000");
        Assert.Equal(1000.123456789m.ToHumanReadableNumber(), "1000");

        Assert.Equal(200.123456789m.ToHumanReadableNumber(), "200.12");
        Assert.Equal(100.123456789m.ToHumanReadableNumber(), "100.12");

        Assert.Equal(20.123456789m.ToHumanReadableNumber(), "20.1235");
        Assert.Equal(10.123456789m.ToHumanReadableNumber(), "10.1235");

        Assert.Equal(2.123456789m.ToHumanReadableNumber(), "2.123457");
        Assert.Equal(1.123456789m.ToHumanReadableNumber(), "1.123457");

        Assert.Equal(0.123456789m.ToHumanReadableNumber(), "0.12345679");
    }
}