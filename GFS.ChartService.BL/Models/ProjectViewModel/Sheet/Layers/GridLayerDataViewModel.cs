namespace GFS.ChartService.BL.Models.ProjectViewModel.Sheet.Layers;

public class GridLayerDataViewModel
{
    public GridLayerProperties Properties { get; init; } = new();
}

public class GridLayerProperties
{
    public bool IsVisible { get; init; } = true;
    public string PremierLinesColor { get; init; } = "Black";
    public string SecondLinesColor { get; init; } = "DarkGray";
    public string ThirdLinesColor { get; init; } = "LightGray";
    public int SecondLinesEvery { get; init; } = 5;
    public int PremierLinesEvery { get; init; } = 10;
}