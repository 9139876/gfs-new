using System.Drawing;

namespace GFS.AnalysisSystem.Library.Calculation.Models;

public class ForecastCalculationResultItem
{
    public ForecastCalculationResultItem(Point position, params string[] descriptions)
    {
        Position = position;
        Descriptions = descriptions?.ToList() ?? new List<string>();
    }

    public Point Position { get; }
    public List<string> Descriptions { get; } 

    public void AddDescription(string description) => Descriptions.Add(description);

    public void AddDescriptionList(IEnumerable<string> descriptions) => Descriptions.AddRange(descriptions);
}