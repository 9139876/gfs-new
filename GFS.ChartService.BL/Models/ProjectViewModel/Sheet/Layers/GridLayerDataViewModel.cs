namespace GFS.ChartService.BL.Models.ProjectViewModel.Sheet.Layers;

public class GridLayerDataViewModel
{
    public GridLayerProperties Properties { get; init; } = new();
}

public class GridLayerProperties
{
    public bool IsVisible { get; init; } = true;
    public string PremierLinesColor { get; init; } = "DarkGreen";
    public string SecondLinesColor { get; init; } = "DarkOliveGreen";
    public string ThirdLinesColor { get; init; } = "DarkOliveGreen";
    public int SecondLinesEvery { get; init; } = 5;
    public int PremierLinesEvery { get; init; } = 10;
}