namespace GFS.AnalysisSystem.Library.Internal.SquareOfNine;

public static class SquareOfNine
{
    private static readonly List<SquareOfNineWheel> Wheels;

    static SquareOfNine()
    {
        Wheels = new List<SquareOfNineWheel>();

        while (Wheels.LastOrDefault()?.LastCellNumber >= ushort.MaxValue == false)
            Wheels.Add(new SquareOfNineWheel((Wheels.LastOrDefault()?.Number ?? 0) + 1));
    }

    public static decimal GetNumberAngle(ushort number)
        => number switch
        {
            <= 0 => throw new ArgumentOutOfRangeException(nameof(number), number.ToString()),
            1 => 0,
            _ => GetNumberWheel(number).GetNumberAngle(number)
        };

    public static decimal GetDegreesBetweenNumbers(ushort number1, ushort number2)
        => Math.Abs(GetNumberAngle(number1) - GetNumberAngle(number2));

    public static decimal GetFullDegreesBetweenNumbers(ushort number1, ushort number2)
        => number1 == number2
            ? 0
            : GetFullDegreesBetweenNumbersInternal(Math.Min(number1, number2), Math.Max(number1, number2));


    private static decimal GetFullDegreesBetweenNumbersInternal(ushort smallNumber, ushort bigNumber)
    {
        var smallWheel = GetNumberWheel(smallNumber);
        var bigWheel = GetNumberWheel(bigNumber);

        if (smallWheel.Number == bigWheel.Number)
            return GetDegreesBetweenNumbers(smallNumber, bigNumber);

        var smallNumberAngle = GetNumberAngle(smallNumber);
        var bigNumberAngle = GetNumberAngle(bigNumber);

        var correction = bigNumberAngle > smallNumberAngle
            ? -1
            : 0;

        return bigNumberAngle - smallNumberAngle + (bigWheel.Number - smallWheel.Number + correction) * 360;
    }

    private static SquareOfNineWheel GetNumberWheel(int number)
    {
        return Wheels.Single(wheel => wheel.FirstCellNumber <= number && wheel.LastCellNumber >= number);
    }
}