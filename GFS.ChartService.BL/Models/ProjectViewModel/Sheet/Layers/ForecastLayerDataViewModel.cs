namespace GFS.ChartService.BL.Models.ProjectViewModel.Sheet.Layers;

public class ForecastLayerDataViewModel
{
    public ForecastLayerProperties Properties { get; init; } = new();
}

public class ForecastPoint
{
    public SheetPointInCell? Point { get; init; }
    public List<ForecastItem> Items { get; init; } = new();
}

public class ForecastItem
{
    public string? Description { get; init; }
}

public class ForecastLayerProperties
{
    public bool IsVisible { get; init; } = true;
}