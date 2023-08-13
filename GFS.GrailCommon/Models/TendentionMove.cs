using GFS.GrailCommon.Enums;

namespace GFS.GrailCommon.Models;

public class TendentionMove
{
    public TendentionMoveDirectionTypeEnum MoveDirectionType { get; }

    public TendentionPoint FirstPoint { get; }

    public TendentionPoint SecondPoint { get; }

    public TendentionMove(TendentionPoint point1, TendentionPoint point2)
    {
        if (point1.Point.Date == point2.Point.Date)
            throw new InvalidOperationException($"Невозможно создать {nameof(TendentionMove)} - даты обеих точек одинаковые");

        var sortedPoints = new[] { point1, point2 }.OrderBy(p => p.Point.Date).ToArray();

        FirstPoint = sortedPoints.First();
        SecondPoint = sortedPoints.Last();

        MoveDirectionType = true switch
        {
            true when SecondPoint.Point.Price > FirstPoint.Point.Price => TendentionMoveDirectionTypeEnum.Up,
            true when SecondPoint.Point.Price < FirstPoint.Point.Price => TendentionMoveDirectionTypeEnum.Down,
            _ => TendentionMoveDirectionTypeEnum.Flat
        };
    }
}