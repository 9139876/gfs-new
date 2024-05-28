namespace GFS.AnalysisSystem.Library.Tendention.Models.BuildTendentionContexts;

public class BuildOutstandingOfNeighborsTendentionContext : BuildTendentionContext
{
    public ByNeighborsType ByNeighborsType { get; init; }
}

public enum ByNeighborsType
{
    ByOneEach = 1,
    ByTwoEach = 2,
    ByThreeEach = 3,
}