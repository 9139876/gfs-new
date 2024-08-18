using GFS.Common.Extensions;
using Xunit;

namespace UnitTests.CommonTests;

public class DecimalExtensionsTests
{
    [Fact]
    public void ToThreeDigitNumber_Test()
    {
        Assert.Equal(300m, 300m.ToThreeDigitNumber());
        Assert.Equal(100m, 100m.ToThreeDigitNumber());
        Assert.Equal(999m, 999m.ToThreeDigitNumber());
        Assert.Equal(100m, 1000m.ToThreeDigitNumber());
        Assert.Equal(100m, 10000m.ToThreeDigitNumber());
        Assert.Equal(100m, 9999999m.ToThreeDigitNumber());
        Assert.Equal(999m, 9991m.ToThreeDigitNumber());
        Assert.Equal(100m, 10m.ToThreeDigitNumber());
        Assert.Equal(100m, 1m.ToThreeDigitNumber());
        Assert.Equal(100m, 0.00001m.ToThreeDigitNumber());
        Assert.Equal(100m, 0.999999m.ToThreeDigitNumber());
        Assert.Equal(999m, 0.9991m.ToThreeDigitNumber());
    }

    [Fact]
    public void ToHumanReadableNumber_Test()
    {
        Assert.Equal("2000", 2000.123456789m.ToHumanReadableNumber());
        Assert.Equal("1000", 1000.123456789m.ToHumanReadableNumber());

        Assert.Equal("200.1", 200.123456789m.ToHumanReadableNumber());
        Assert.Equal("100.1", 100.123456789m.ToHumanReadableNumber());

        Assert.Equal("20.12", 20.123456789m.ToHumanReadableNumber());
        Assert.Equal("10.12", 10.123456789m.ToHumanReadableNumber());

        Assert.Equal("2.123", 2.123456789m.ToHumanReadableNumber());
        Assert.Equal("1.123", 1.123456789m.ToHumanReadableNumber());

        Assert.Equal("0.123457", 0.123456789m.ToHumanReadableNumber());
    }
}