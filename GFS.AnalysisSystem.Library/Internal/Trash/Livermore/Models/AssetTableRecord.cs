using GFS.AnalysisSystem.Library.Internal.Trash.Livermore.Enum;
using GFS.GrailCommon.Models;

namespace GFS.AnalysisSystem.Library.Internal.Trash.Livermore.Models;

internal class AssetTableRecord
{
    public DateTime Date { get; }

    public TableColumnTypeEnum Column { get; }

    public decimal Value { get; }

    public bool Underlined { get; private set; }

    public void Underline() => Underlined = true;
    
    public AssetTableRecord(DateTime date, TableColumnTypeEnum column, decimal value)
    {
        Date = date;
        Column = column;
        Value = value;
    }

    public AssetTableRecord(PriceTimePoint point, TableColumnTypeEnum column)
    {
        Date = point.Date;
        Column = column;
        Value = point.Price;
    }
}