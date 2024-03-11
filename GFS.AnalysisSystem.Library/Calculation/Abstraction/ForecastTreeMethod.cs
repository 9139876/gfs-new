using GFS.AnalysisSystem.Library.Calculation.Models;

namespace GFS.AnalysisSystem.Library.Calculation.Abstraction;

public interface IForecastTreeMethod : IForecastTreeItem
{
    // IForecastTreeItem ParentGroup { get; }
    ForecastCalculationResult Calculate(CalculationContext context);
}

public abstract class ForecastTreeMethod<TParentGroup> : IForecastTreeMethod
    where TParentGroup : IForecastTreeGroup
{
    public abstract ForecastCalculationResult Calculate(CalculationContext context);

    // public IForecastTreeItem ParentGroup => ForecastCalculator.Groups.SingleOrDefault(g => g.GetType() == typeof(TParentGroup)) as IForecastTreeItem
    //                                         ?? throw new InvalidOperationException($"Fail to get parent group {typeof(TParentGroup).Name} for {GetType().Name}");

    public string Identifier => GetType().Name;
    public abstract string Name { get; }
}