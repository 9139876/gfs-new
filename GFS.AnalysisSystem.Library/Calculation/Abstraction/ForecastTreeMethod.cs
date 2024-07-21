using GFS.AnalysisSystem.Library.Calculation.Models;

namespace GFS.AnalysisSystem.Library.Calculation.Abstraction;

public interface IForecastTreeMethod : IForecastTreeItem
{
    ForecastCalculationResult Calculate(CalculationContext context);
}

// ReSharper disable once UnusedTypeParameter
public abstract class ForecastTreeMethod<TParentGroup> : IForecastTreeMethod
    where TParentGroup : IForecastTreeGroup
{
    public abstract ForecastCalculationResult Calculate(CalculationContext context);

    public string Identifier => GetType().Name;

    public abstract string Name { get; }
}