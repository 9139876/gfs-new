using GFS.AnalysisSystem.Library.Livermore.Enum;
using GFS.AnalysisSystem.Library.Livermore.Models;
using GFS.GrailCommon.Models;

namespace GFS.AnalysisSystem.Library.Livermore;

public class LivermoreAnalyzeService
{
    private readonly List<AssetTable> _assetTables;

    private readonly Func<QuoteModel, PriceTimePoint> _quoteToPriceTimePoint;

    public LivermoreAnalyzeService(IList<(Guid, LivermoreAnalyzeSettings)> initialData)
    {
        _assetTables = initialData.Select(id => new AssetTable(id.Item1, id.Item2)).ToList();

        _quoteToPriceTimePoint = initialData.First().Item2.QuoteToPriceConverterType switch
        {
            QuoteToPriceConverterTypeEnum.Close => q => new PriceTimePoint { Date = q.Date, Price = q.Close },
            QuoteToPriceConverterTypeEnum.HiLowMedian => q => new PriceTimePoint { Date = q.Date, Price = (q.High + q.Low) / 2 },
            _ => throw new ArgumentOutOfRangeException(nameof(QuoteToPriceConverterTypeEnum))
        };
    }

    public void HandleQuote(Guid assetId, QuoteModel quote)
    {
        var table = _assetTables.Single(t => t.AssetId == assetId);
        table.HandleQuote(_quoteToPriceTimePoint(quote));
    }
}