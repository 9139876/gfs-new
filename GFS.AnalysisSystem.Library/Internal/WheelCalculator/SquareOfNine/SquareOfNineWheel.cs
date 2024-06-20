namespace GFS.AnalysisSystem.Library.Internal.WheelCalculator.SquareOfNine;

public class SquareOfNineWheel : IWheel
{
    private int _firstCellNumber;
    private int _lastCellNumber;
    private SquareOfNineWheelLine[]? _lines;

    public int Number { get; private set; }
    
    public IWheel Create(IWheel? previous)
    {
        var instance = new SquareOfNineWheel();

        instance.Number = (previous?.Number ?? 0) + 1;

        instance._firstCellNumber = instance.Number == 1 ? 1 : (int)Math.Pow((instance.Number - 1) * 2 - 1, 2) + 1;
        instance._lastCellNumber = (int)Math.Pow(instance.Number * 2 - 1, 2);

        var (cardinalNumbers, diagonalNumbers) = instance.GetCrosses();

        instance._lines = new SquareOfNineWheelLine[]
        {
            new(1, _firstCellNumber, cardinalNumbers[0], diagonalNumbers[0], instance),
            new(2, diagonalNumbers[0], cardinalNumbers[1], diagonalNumbers[1], instance),
            new(3, diagonalNumbers[1], cardinalNumbers[2], diagonalNumbers[2], instance),
            new(4, diagonalNumbers[2], cardinalNumbers[3], diagonalNumbers[3], instance),
        };

        return instance;
    }

    public bool IsLastWheel()
        => _lastCellNumber >= ushort.MaxValue;
    
    public bool ContainNumber(int number)
    {
        return number >= _firstCellNumber && number <= _lastCellNumber;
    }

    public decimal GetNumberAngle(int number)
    {
        if (!ContainNumber(number))
            throw new ArgumentOutOfRangeException(nameof(number), number.ToString());

        return _lines!.First(line => line.ContainNumber(number)).GetNumberAngle(number);
    }

    private (int[], int[]) GetCrosses()
    {
        var cardinalNumbers = new int[4];
        for (var i = 0; i < 4; i++)
            cardinalNumbers[i] = _firstCellNumber + Number - 2 + 2 * (Number - 1) * i;

        var diagonalNumbers = new int[4];
        for (var i = 0; i < 4; i++)
            diagonalNumbers[i] = _firstCellNumber + 2 * (Number - 1) - 1 + 2 * (Number - 1) * i;

        return (cardinalNumbers, diagonalNumbers);
    }
}