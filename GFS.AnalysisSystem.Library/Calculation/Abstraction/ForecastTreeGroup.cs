namespace GFS.AnalysisSystem.Library.Calculation.Abstraction;

public interface IForecastTreeGroup : IForecastTreeItem
{
    List<IForecastTreeItem> Children { get; }
}

// ReSharper disable once UnusedTypeParameter
public abstract class ForecastTreeGroup<TParentGroup> : IForecastTreeGroup
    where TParentGroup : IForecastTreeGroup
{
    public List<IForecastTreeItem> Children { get; } = new();

    public string Identifier => GetType().Name;

    public abstract string Name { get; }
}

public class ForecastTreeRoot : IForecastTreeGroup
{
    public List<IForecastTreeItem> Children { get; } = new();
    public string Identifier => GetType().Name;
    public string Name => "root";
}