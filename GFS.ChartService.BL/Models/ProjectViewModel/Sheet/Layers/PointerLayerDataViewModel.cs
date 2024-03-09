namespace GFS.ChartService.BL.Models.ProjectViewModel.Sheet.Layers;

public class PointerLayerDataViewModel
{
    public PointerLayerProperties Properties { get; init; } = new();
    public SheetPointInCell? TargetPoint { get; init; }
    public List<SheetPointInCell> SourcePoints { get; init; } = new();
}

public class PointerLayerProperties
{
    public bool IsVisible { get; init; } = true;
    public string TargetPointColor { get; init; } = "red";
    public string SourcePointColor { get; init; } = "lime";
    public int PointsSize { get; init; } = 2;
    public string LinesColor { get; init; } = "lime";
    public int LinesWight { get; init; } = 2;
}