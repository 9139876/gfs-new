namespace GFS.BalanceCalculation.BL.Abstractions
{
    public interface IGroupedItem
    {
        IBalanceCalculationMethodsGroup? ParentGroup { get; }
        string? FullName { get; }
        string? OwnName { get; }
    }
}