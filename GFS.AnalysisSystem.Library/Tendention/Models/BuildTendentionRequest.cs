using GFS.AnalysisSystem.Library.Tendention.Models.BuildTendentionContexts;
using GFS.GrailCommon.Models;

namespace GFS.AnalysisSystem.Library.Tendention.Models;

public class BuildTendentionRequest<TContext>
    where TContext : BuildTendentionContext
{
    public IList<CandleInCells> Quotes { get; }

    public TContext Context { get; }

    public BuildTendentionRequest(IList<CandleInCells> quotes, TContext context)
    {
        Quotes = quotes;
        Context = context;
    }
}