using GFS.AnalysisSystem.Library.Livermore.Models;
using GFS.GrailCommon.Models;

namespace GFS.AnalysisSystem.Library.Livermore;

public class LivermoreAnalyzeService
{
    private readonly List<AssetTable> _assetTables;

    public LivermoreAnalyzeService(IEnumerable<(Guid, LivermoreAnalyzeSettings)> initialData)
    {
        _assetTables = initialData.Select(id => new AssetTable(id.Item1, id.Item2)).ToList();
    }

    public void HandleQuote(Guid assetId, QuoteModel quote)
    {
        var table = _assetTables.Single(t => t.AssetId == assetId);
        table.HandleQuote(quote);
    }
    
}