namespace GFS.AnalysisSystem.Library.Internal.WheelCalculator;

public static class WheelCalculator<TWheel>
    where TWheel : AbstractWheel
{
    private static readonly List<TWheel> Wheels;

    static WheelCalculator()
    {
        Wheels = new List<TWheel>();

        while (Wheels.LastOrDefault()?.IsLastWheel() != true)
            Wheels.Add(Activator.CreateInstance(typeof(TWheel), Wheels.LastOrDefault()) as TWheel ??
                       throw new InvalidOperationException($"Ошибка создания экземпляра {typeof(TWheel).Name}"));
    }

    public static decimal GetNumberAngle(ushort number)
        => GetNumberWheel(number).GetNumberAngle(number);

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

    private static TWheel GetNumberWheel(int number)
    {
        return Wheels.Single(wheel => wheel.ContainNumber(number));
    }
}