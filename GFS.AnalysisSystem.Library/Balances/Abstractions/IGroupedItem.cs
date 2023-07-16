namespace GFS.AnalysisSystem.Library.Balances.Abstractions
{
    public interface IGroupedItem
    {
        IBalanceCalculationMethodsGroup? ParentGroup { get; }
        string? FullName { get; }
        string? OwnName { get; }
    }
}