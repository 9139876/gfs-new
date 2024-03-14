namespace GFS.AnalysisSystem.Library.Calculation.Models;

public class ForecastCalculationResult
{
    private readonly List<ForecastCalculationResultItem> _items = new();

    public void AddForecastCalculationResultItem(ForecastCalculationResultItem item)
    {
        _items.Add(item);
    }

    public void AddForecastCalculationResultItemList(IEnumerable<ForecastCalculationResultItem> items)
    {
        _items.AddRange(items);
    }

    public List<ForecastCalculationResultItem> GetItems => _items.ToList();
}