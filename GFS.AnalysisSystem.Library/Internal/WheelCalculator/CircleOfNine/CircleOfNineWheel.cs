namespace GFS.AnalysisSystem.Library.Internal.WheelCalculator.CircleOfNine;

public class CircleOfNineWheel : IWheel
{
    private int _firstCellNumber;
    private int _lastCellNumber;
    private decimal DegreeCellSize;
    
    public int Number { get; private set; }
    

    public IWheel Create(IWheel? previous)
    {
        var instance = new CircleOfNineWheel();

        instance.Number = (previous?.Number ?? 0) + 1;

        instance._firstCellNumber = instance.Number == 1 ? 1 : (int)Math.Pow((instance.Number - 1) * 2 - 1, 2) + 1;
        instance._lastCellNumber = (int)Math.Pow(instance.Number * 2 - 1, 2);
        instance.DegreeCellSize = (decimal)360 / (instance._lastCellNumber - instance._firstCellNumber + 1);

        return instance;
    }

    public bool IsLastWheel()
        => _lastCellNumber >= ushort.MaxValue;
    
    public decimal GetNumberAngle(int number)
    {
        if (!ContainNumber(number))
            throw new ArgumentOutOfRangeException(nameof(number), number.ToString());
        
        return DegreeCellSize * (number - _firstCellNumber + 0.5m);
    }

    public bool ContainNumber(int number)
    {
        return number >= _firstCellNumber && number <= _lastCellNumber;
    }
}