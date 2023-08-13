using GFS.GrailCommon.Enums;

namespace GFS.GrailCommon.Models;

public class Tendention
{
    private readonly List<TendentionPoint> _points = new();
    private readonly List<TendentionMove> _moves = new();
    private bool _isCorrect;
    private TendentionMoveDirectionTypeEnum _nextDirection = TendentionMoveDirectionTypeEnum.Flat;

    public bool IsCorrect
    {
        get => _isCorrect;
        private set
        {
            if (value == false)
            {
                _points.ForEach(p => p.SetPointType(TendentionPointTypeEnum.Unknown));
                _nextDirection = TendentionMoveDirectionTypeEnum.Flat;
                _moves.Clear();
            }

            _isCorrect = value;
        }
    }

    public void AddPoint(PriceTimePoint point)
    {
        var existingPoint = _points.SingleOrDefault(p => p.Point.Date == point.Date);

        if (existingPoint is not null)
            _points.Remove(existingPoint);

        _points.Add(new TendentionPoint(point));
        _points.Sort(new TendentionPointComparer());

        RecalculateTendention();
    }

    public bool RemovePoint(DateTime date)
    {
        var existingPoint = _points.SingleOrDefault(p => p.Point.Date == date);

        if (existingPoint is null)
            return false;

        _points.Remove(existingPoint);
        RecalculateTendention();
        return true;
    }

    public bool TryGetNextDirection(out TendentionMoveDirectionTypeEnum nextDirection)
    {
        nextDirection = _nextDirection;
        return _isCorrect;
    }

    public bool TryGetMoves(out List<TendentionMove> moves)
    {
        moves = _isCorrect ? _moves : new List<TendentionMove>();
        return _isCorrect;
    }

    public bool TryGetPoints(out List<TendentionPoint> points)
    {
        points = _isCorrect ? _points : new List<TendentionPoint>();
        return _isCorrect;
    }

    private void RecalculateTendention()
    {
        if (_points.Count < 2)
        {
            IsCorrect = false;
            return;
        }

        _moves.Clear();

        for (var i = 0; i < _points.Count - 1; i++)
            _moves.Add(new TendentionMove(_points[i], _points[i + 1]));

        if (_moves.Any(m => m.MoveDirectionType == TendentionMoveDirectionTypeEnum.Flat))
        {
            IsCorrect = false;
            return;
        }

        var firstMove = _moves.First();

        var currentDirection = firstMove.MoveDirectionType;
        firstMove.FirstPoint.SetPointType(currentDirection == TendentionMoveDirectionTypeEnum.Up ? TendentionPointTypeEnum.Bottom : TendentionPointTypeEnum.Top);
        firstMove.SecondPoint.SetPointType(currentDirection == TendentionMoveDirectionTypeEnum.Up ? TendentionPointTypeEnum.Top : TendentionPointTypeEnum.Bottom);

        foreach (var move in _moves.Skip(1))
        {
            if (move.MoveDirectionType == currentDirection)
            {
                IsCorrect = false;
                return;
            }

            currentDirection = move.MoveDirectionType;
            move.SecondPoint.SetPointType(currentDirection == TendentionMoveDirectionTypeEnum.Up ? TendentionPointTypeEnum.Top : TendentionPointTypeEnum.Bottom);
        }

        _nextDirection = currentDirection == TendentionMoveDirectionTypeEnum.Up ? TendentionMoveDirectionTypeEnum.Down : TendentionMoveDirectionTypeEnum.Up;
        IsCorrect = true;
    }
}