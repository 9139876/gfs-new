namespace GFS.AnalysisSystem.Library.Internal.SquareOfNine;

public class SquareOfNineWheelLine
{
    private readonly int _number;
    private readonly int _firstCellNumber;
    private readonly int _middleCellNumber;
    private readonly int _lastCellNumber;
    private readonly int _middleCellDegree;
    private readonly SquareOfNineWheel _squareOfNineWheel;

    public SquareOfNineWheelLine(
        int number,
        int firstCellNumber,
        int middleCellNumber,
        int lastCellNumber,
        SquareOfNineWheel squareOfNineWheel)
    {
        _number = number;
        _firstCellNumber = firstCellNumber;
        _middleCellNumber = middleCellNumber;
        _lastCellNumber = lastCellNumber;
        _middleCellDegree = GetMiddleCellDegree();
        _squareOfNineWheel = squareOfNineWheel;
    }

    public bool ContainNumber(int number)
    {
        return number >= _firstCellNumber && number <= _lastCellNumber;
    }

    public decimal GetNumberAngle(int number)
    {
        if (!ContainNumber(number))
            throw new ArgumentOutOfRangeException(nameof(number), number.ToString());

        if (number == _lastCellNumber)
            return _middleCellDegree + 45;

        if (number == _firstCellNumber && _number != 1)
            return _middleCellDegree - 45;

        //По центру клеточки
        double x = _squareOfNineWheel.Number - 1;
        double y = number - _middleCellNumber;

        var alpha = (decimal)(Math.Atan(y / x) * 360 / (2 * Math.PI));

        return _middleCellDegree + alpha;
    }

    private int GetMiddleCellDegree()
    {
        return _number switch
        {
            1 => 0,
            2 => 90,
            3 => 180,
            4 => 270,
            _ => throw new ArgumentOutOfRangeException(nameof(_number), _number.ToString())
        };
    }
}