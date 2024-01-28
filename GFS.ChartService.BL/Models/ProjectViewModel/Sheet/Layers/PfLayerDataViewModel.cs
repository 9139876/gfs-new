namespace GFS.ChartService.BL.Models.ProjectViewModel.Sheet.Layers;

public class PfLayerDataViewModel
{
    public PfLayerProperties Properties { get; init; } = new();
    public List<PointForceViewModel> Points { get; init; } = new();
}

public class PfLayerProperties
{
    public bool IsVisible { get; init; } = true;

    public string LinesColor { get; init; } = "green";
    public string PointsColor { get; init; } = "blue";

    public int LinesWight { get; init; } = 1;
    public int PointsSize { get; init; } = 1;
}

public class PointForceViewModel : SheetPointInCell
{
    public Guid Index { get; init; }
    public bool IsCorrect { get; init; }
}