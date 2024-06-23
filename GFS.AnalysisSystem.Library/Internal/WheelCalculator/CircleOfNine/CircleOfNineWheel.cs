namespace GFS.AnalysisSystem.Library.Internal.WheelCalculator.CircleOfNine;

public class CircleOfNineWheel : AbstractWheel
{
    private readonly decimal _degreeCellSize;

    public CircleOfNineWheel(AbstractWheel? previous) : base(previous)
    {
        _degreeCellSize = (decimal)360 / (LastCellNumber - FirstCellNumber + 1);
    }

    protected override decimal GetNumberAngleInternal(int number)
        => number == 1 ? 0 : _degreeCellSize * (number - FirstCellNumber + 0.5m);

    protected override uint GetLastCellNumber(ushort number)
        => (uint)Math.Pow(number * 2 - 1, 2);
}