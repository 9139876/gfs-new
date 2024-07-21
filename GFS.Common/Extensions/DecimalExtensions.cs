namespace GFS.Common.Extensions;

public static class DecimalExtensions
{
    public static decimal RoundToTwoDecimalPlaces(this decimal number)
        => Math.Round(number, 2, MidpointRounding.AwayFromZero);

    public static decimal ToThreeDigitNumber(this decimal number)
    {
        while (true)
        {
            while (number < 100) number *= 10;

            while (number >= 1000) number /= 10;

            var result = Math.Round(number);

            if (result is >= 100 and < 1000)
                return result;

            number = result;
        }
    }

    public static string ToHumanReadableNumber(this decimal number)
    {
        return number switch
        {
            >= 1000 => $"{number:0}",
            >= 100 => $"{number:0.#}",
            >= 10 => $"{number:0.##}",
            >= 1 => $"{number:0.###}",
            _ => $"{number:0.######}",
        };
    }
}