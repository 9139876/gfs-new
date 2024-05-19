using GFS.GrailCommon.Enums;

namespace GFS.GrailCommon.Models;

public class TendentionMove
{
    public PriceMoveDirectionTypeEnum MoveDirectionType { get; }

    public TendentionPoint Begin { get; }

    public TendentionPoint End { get; }

    /// <summary> Величина движения по цене </summary>
    public decimal PriceMove => End.Point.Y - Begin.Point.Y;

    /// <summary> Величина движения по времени </summary>
    public int TimeMove => End.Point.X - Begin.Point.X;
    
    public TendentionMove(TendentionPoint point1, TendentionPoint point2)
    {
        if (point1.Point.X == point2.Point.X)
            throw new InvalidOperationException($"Невозможно создать {nameof(TendentionMove)} - даты обеих точек одинаковые");

        var sortedPoints = new[] { point1, point2 }.OrderBy(p => p.Point.X).ToArray();

        Begin = sortedPoints.First();
        End = sortedPoints.Last();

        MoveDirectionType = true switch
        {
            true when End.Point.Y > Begin.Point.Y => PriceMoveDirectionTypeEnum.Up,
            true when End.Point.Y < Begin.Point.Y => PriceMoveDirectionTypeEnum.Down,
            _ => PriceMoveDirectionTypeEnum.Unknown
        };
    }
}