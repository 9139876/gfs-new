namespace GFS.Common.Extensions;

public static class DecimalExtensions
{
    public static decimal RoundToTwoDecimalPlaces(this decimal number)
        => Math.Round(number, 2, MidpointRounding.AwayFromZero);

}