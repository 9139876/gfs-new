namespace GFS.AnalysisSystem.Library.Internal.WheelCalculator.Hexagon;

public class HexagonWheel : AbstractWheel
{
    private readonly decimal _degreeCellSize;

    public HexagonWheel(AbstractWheel? previous) : base(previous)
    {
        _degreeCellSize = (decimal)360 / (LastCellNumber - FirstCellNumber + 1);
    }

    protected override decimal GetNumberAngleInternal(int number)
    {
        if (number == LastCellNumber)
            return 0;

        return 360 - (LastCellNumber - number) * _degreeCellSize;
    }

    protected override uint GetLastCellNumber(ushort number)
        => (uint)(6 * number * (number + 1) / 2);
}