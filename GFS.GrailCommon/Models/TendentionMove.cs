using GFS.GrailCommon.Enums;

namespace GFS.GrailCommon.Models;

public class TendentionMove
{
    public PriceMoveDirectionTypeEnum MoveDirectionType { get; }

    public TendentionPoint Begin { get; }

    public TendentionPoint End { get; }

    /// <summary> Величина движения по цене </summary>
    public decimal PriceMove => End.Point.Price - Begin.Point.Price;

    /// <summary> Величина движения по времени </summary>
    public TimeSpan TimeMove => End.Point.Date - Begin.Point.Date;
    
    public TendentionMove(TendentionPoint point1, TendentionPoint point2)
    {
        if (point1.Point.Date == point2.Point.Date)
            throw new InvalidOperationException($"Невозможно создать {nameof(TendentionMove)} - даты обеих точек одинаковые");

        var sortedPoints = new[] { point1, point2 }.OrderBy(p => p.Point.Date).ToArray();

        Begin = sortedPoints.First();
        End = sortedPoints.Last();

        MoveDirectionType = true switch
        {
            true when End.Point.Price > Begin.Point.Price => PriceMoveDirectionTypeEnum.Up,
            true when End.Point.Price < Begin.Point.Price => PriceMoveDirectionTypeEnum.Down,
            _ => PriceMoveDirectionTypeEnum.Flat
        };
    }
}