using GFS.AnalysisSystem.Library.Tendention.Models.BuildTendentionContexts;
using GFS.GrailCommon.Models;

namespace GFS.AnalysisSystem.Library.Tendention.TendentionBuilders;

public abstract class FromCandlesTendentionBuilder<TContext>
    where TContext : BuildTendentionContext
{
    protected readonly TContext Context;
    protected readonly IList<CandleInCells> Candles;

    protected FromCandlesTendentionBuilder(TContext context, IList<CandleInCells> candles)
    {
        ValidateQuotes(candles);
        
        Context = context;
        Candles = candles;
    }
    
    public abstract GrailCommon.Models.Tendention BuildThreePointsTendention();
    
    private static void ValidateQuotes(IEnumerable<CandleInCells> candles)
    {
        var duplicateGroup = candles.GroupBy(q => q.Date).FirstOrDefault(gr => gr.Count() > 1);

        if (duplicateGroup is not null)
            throw new InvalidOperationException($"В коллекции котировок, переданных построителю тенденции, более одной котировки на дату {duplicateGroup.Key}");
    }
}