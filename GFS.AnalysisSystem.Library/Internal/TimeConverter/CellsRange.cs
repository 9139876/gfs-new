namespace GFS.AnalysisSystem.Library.Internal.TimeConverter;

public class CellsRange
{
    public CellsRange(ushort minValue, ushort maxValue)
    {
        MinValue = minValue;
        MaxValue = maxValue;
    }

    public ushort MinValue { get; }
    public ushort MaxValue { get; }
}