using System.Drawing;

namespace GFS.AnalysisSystem.Library.Calculation.Models;

public class ForecastCalculationResultItem
{
    public ForecastCalculationResultItem(Point position, params string[] descriptions)
    {
        Position = position;
        Descriptions = descriptions.ToList();
    }

    public Point Position { get; }
    public List<string> Descriptions { get; } 

    public void AddDescription(string description) => Descriptions.Add(description);

    public void AddDescriptionList(IEnumerable<string> descriptions) => Descriptions.AddRange(descriptions);
}

internal class ComparerForecastCalculationResultItemByPosition : IEqualityComparer<ForecastCalculationResultItem>
{
    public bool Equals(ForecastCalculationResultItem? x, ForecastCalculationResultItem? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.Position.Equals(y.Position);
    }

    public int GetHashCode(ForecastCalculationResultItem obj)
    {
        return obj.Position.GetHashCode();
    }
}