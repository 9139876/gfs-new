using GFS.AnalysisSystem.Library.Internal.Trash.Livermore.Enum;
using GFS.GrailCommon.Models;

namespace GFS.AnalysisSystem.Library.Internal.Trash.Livermore.Models;

internal class AssetTable
{
    private readonly List<DateTime> _recordDates = new List<DateTime>();
    private readonly List<AssetTableRecord> _records;
    private readonly LivermoreRecordHandler _handler;

    private readonly Func<PriceTimePoint, IPriceValue> _pointToPriceValue;

    public Guid AssetId { get; }

    public AssetTable(Guid assetId, LivermoreAnalyzeSettings settings)
    {
        AssetId = assetId;

        _pointToPriceValue = settings.PriceComparisonType switch
        {
            PriceComparisonTypeEnum.Absolute => p => new PriceValueAbsoluteComparison(p.Price, settings.KPrice),
            PriceComparisonTypeEnum.Percentage => p => new PriceValuePercentageComparison(p.Price),
            _ => throw new ArgumentOutOfRangeException(nameof(settings.PriceComparisonType))
        };

        _handler = new LivermoreRecordHandler(this);
        _records = new List<AssetTableRecord>();
    }

    private DateTime LastRecordDate => _recordDates.LastOrDefault();
    
    public void HandleQuote(PriceTimePoint point)
    {
        if (LastRecordDate >= point.Date)
            throw new InvalidOperationException($"Попытка обработать котировку на дату {point.Date:g}, когда уже была обработана за {LastRecordDate:g}");

        _recordDates.Add(point.Date);

        var column = _handler.Handle(_pointToPriceValue(point));

        if (column != null)
            AddRecord(new AssetTableRecord(point, column.Value));
    }

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

    public DateTime[] GetAllRecordDates() => _recordDates.ToArray();

    public AssetTableRecord? GetRecordByDate(DateTime date) => _records.SingleOrDefault(r => r.Date == date);
    
    private void AddRecord(AssetTableRecord record) => _records.Add(record);

    private AssetTableRecord LastRecord() => _records.Last();

    private AssetTableRecord LastRecord(TableColumnTypeEnum column) => _records.Last(r => r.Column == column);
}