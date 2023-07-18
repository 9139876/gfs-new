using GFS.AnalysisSystem.Library.Livermore.Enum;
using GFS.GrailCommon.Models;

namespace GFS.AnalysisSystem.Library.Livermore.Models;

public class AssetTable
{
    private readonly List<AssetTableRecord> _records;
    private readonly LivermoreRecordHandler _handler;

    private readonly Func<QuoteModel, PriceTimePoint> _quoteToPoint;

    public Guid AssetId { get; }
    
    public AssetTable(Guid assetId, LivermoreAnalyzeSettings settings)
    {
        AssetId = assetId;
        _quoteToPoint = settings.QuoteToPriceConverterType switch
        {
            QuoteToPriceConverterTypeEnum.Close => q => new PriceTimePoint { Date = q.Date, Price = q.Close },
            QuoteToPriceConverterTypeEnum.HiLowMedian => q => new PriceTimePoint { Date = q.Date, Price = (q.High + q.Low) / 2 },
            _ => throw new ArgumentOutOfRangeException()
        };

        _handler = new LivermoreRecordHandler(this, (one, two) => Math.Abs(one - two) >= 6 / settings.KPrice);
        _records = new List<AssetTableRecord>();
    }

    public void HandleQuote(QuoteModel quote) => _handler.Handle(_quoteToPoint(quote));

    public void AddRecord(AssetTableRecord record) => _records.Add(record);

    public void UnderlineLastRecord(TableColumnTypeEnum column) => LastRecord(column).Underline();
    
    public TableColumnTypeEnum CurrentColumn() => LastRecord().Column;

    public decimal LastValue() => _records.Last().Value;

    public decimal ColumnLastUnderlinedValue(TableColumnTypeEnum column)
    {
        var value = _records.LastOrDefault(r => r.Column == column && r.Underlined)?.Value;

        if (value.HasValue)
            return value.Value;

        return new[] { TableColumnTypeEnum.DownwardTrend, TableColumnTypeEnum.NaturalReaction, TableColumnTypeEnum.SecondaryReaction }.Contains(column)
            ? decimal.MaxValue
            : decimal.MinValue;
    }
    
    private AssetTableRecord LastRecord() => _records.Last();

    private AssetTableRecord LastRecord(TableColumnTypeEnum column) => _records.Last(r => r.Column == column);
}