namespace GFS.AnalysisSystem.Library.Balances.Abstractions
{
    public interface IBalanceCalculationMethodsGroup : IGroupedItem
    {
        void AddChildMethod(IBalanceCalculationMethod method);
        void AddChildGroup(IBalanceCalculationMethodsGroup group);
        IBalanceCalculationMethod? TryGetMethod(Guid id);
        // IBalanceCalculationMethodsGroup? TryGetGroup<TGroup>() where TGroup : IBalanceCalculationMethodsGroup;
        // IBalanceCalculationMethodsGroup? TryGetGroup(Predicate<IBalanceCalculationMethodsGroup> predicate);
        // IBalanceCalculationMethod? TryGetMethod<TMethod>() where TMethod : IBalanceCalculationMethod;
        // IBalanceCalculationMethod? TryGetMethod(Predicate<IBalanceCalculationMethod> predicate);
    }
}