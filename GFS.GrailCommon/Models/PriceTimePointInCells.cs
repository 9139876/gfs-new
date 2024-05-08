namespace GFS.GrailCommon.Models;

/// <summary> Позиция в координатах цена время в клеточках</summary>
public class PriceTimePointInCells
{
    /// <summary> Дата </summary>
    public int X { get; init; }

    /// <summary> Цена </summary>
    public int Y { get; init; }
}