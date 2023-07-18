using GFS.AnalysisSystem.Library.Livermore.Enum;
using GFS.GrailCommon.Models;

namespace GFS.AnalysisSystem.Library.Livermore.Models;

public class AssetTable
{
    private readonly List<AssetTableRecord> _records;
    private readonly LivermoreRecordHandler _handler;

    private readonly Func<QuoteModel, PriceValue> _quoteToPriceValue;

    public Guid AssetId { get; }

    public AssetTable(Guid assetId, LivermoreAnalyzeSettings settings)
    {
        AssetId = assetId;
        _quoteToPriceValue = settings.QuoteToPriceConverterType switch
        {
            QuoteToPriceConverterTypeEnum.Close => q => new PriceValue(price: q.Close, kPrice: settings.KPrice),
            QuoteToPriceConverterTypeEnum.HiLowMedian => q => new PriceValue(price: (q.High + q.Low) / 2, kPrice: settings.KPrice),
            _ => throw new ArgumentOutOfRangeException()
        };

        _handler = new LivermoreRecordHandler(this);
        _records = new List<AssetTableRecord>();
    }

    public void HandleQuote(QuoteModel quote)
    {
        var column = _handler.Handle(_quoteToPriceValue(quote));

        if (column != null)
            AddRecord(new AssetTableRecord(_quoteToPriceValue(quote), column.Value));
    }

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

public class PriceValue
{
    private readonly decimal _price;
    private readonly decimal _kPrice;

    public PriceValue(decimal price, decimal kPrice)
    {
        _price = price;
        _kPrice = kPrice;
    }

    public bool IsMoreOnSixPointsThan(decimal value) => false;
    public bool IsMoreOnThreePointsThan(decimal value) => false;
    public bool IsLessOnSixPointsThan(decimal value) => false;
    public bool IsLessOnThreePointsThan(decimal value) => false;
    public bool IsMoreThan(decimal value) => false;
    public bool IsLessThan(decimal value) => false;
}