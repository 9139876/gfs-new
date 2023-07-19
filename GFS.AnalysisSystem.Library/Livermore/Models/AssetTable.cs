using GFS.AnalysisSystem.Library.Livermore.Enum;
using GFS.GrailCommon.Models;

namespace GFS.AnalysisSystem.Library.Livermore.Models;

public class AssetTable
{
    private readonly List<AssetTableRecord> _records;
    private readonly LivermoreRecordHandler _handler;

    private readonly Func<PriceTimePoint, IPriceValue> _pointToPriceValue;

    private DateTime _lastRecordDate = DateTime.MinValue;

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

    public void HandleQuote(PriceTimePoint point)
    {
        if (_lastRecordDate >= point.Date)
            throw new InvalidOperationException($"Попытка обработать котировку на дату {point.Date:g}, когда уже была обработана за {_lastRecordDate:g}");

        _lastRecordDate = point.Date;

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

    private void AddRecord(AssetTableRecord record) => _records.Add(record);

    private AssetTableRecord LastRecord() => _records.Last();

    private AssetTableRecord LastRecord(TableColumnTypeEnum column) => _records.Last(r => r.Column == column);
}