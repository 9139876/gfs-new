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
    {
        var wheel1 = GetNumberWheel(number1);
        var wheel2 = GetNumberWheel(number2);

        return Math.Abs(GetNumberAngle(number2) - GetNumberAngle(number1)) + Math.Abs(wheel2.Number - wheel1.Number) * 360;
    }

    private static SquareOfNineWheel GetNumberWheel(int number)
    {
        return Wheels.Single(wheel => wheel.FirstCellNumber <= number && wheel.LastCellNumber >= number);
    }
}