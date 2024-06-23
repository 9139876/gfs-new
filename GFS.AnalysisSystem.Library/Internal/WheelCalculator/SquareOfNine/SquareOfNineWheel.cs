namespace GFS.AnalysisSystem.Library.Internal.WheelCalculator.SquareOfNine;

public class SquareOfNineWheel : AbstractWheel
{
    private SquareOfNineWheelLine[]? _lines;

    public SquareOfNineWheel(AbstractWheel? previous) : base(previous)
    {
        var (cardinalNumbers, diagonalNumbers) = GetCrosses();

        _lines = new SquareOfNineWheelLine[]
        {
            new(1, (int)FirstCellNumber, cardinalNumbers[0], diagonalNumbers[0], this),
            new(2, diagonalNumbers[0], cardinalNumbers[1], diagonalNumbers[1], this),
            new(3, diagonalNumbers[1], cardinalNumbers[2], diagonalNumbers[2], this),
            new(4, diagonalNumbers[2], cardinalNumbers[3], diagonalNumbers[3], this),
        };
    }

    protected override decimal GetNumberAngleInternal(int number)
        => number == 1 ? 0 : _lines!.First(line => line.ContainNumber(number)).GetNumberAngle(number);

    protected override uint GetLastCellNumber(ushort number)
        => (uint)Math.Pow(number * 2 - 1, 2);

    private (int[], int[]) GetCrosses()
    {
        var cardinalNumbers = new int[4];
        for (var i = 0; i < 4; i++)
            cardinalNumbers[i] = (int)FirstCellNumber + Number - 2 + 2 * (Number - 1) * i;

        var diagonalNumbers = new int[4];
        for (var i = 0; i < 4; i++)
            diagonalNumbers[i] = (int)FirstCellNumber + 2 * (Number - 1) - 1 + 2 * (Number - 1) * i;

        return (cardinalNumbers, diagonalNumbers);
    }
}