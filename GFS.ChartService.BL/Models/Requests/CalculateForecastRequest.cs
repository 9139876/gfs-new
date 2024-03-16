using GFS.ChartService.BL.Models.ProjectViewModel.Sheet;

namespace GFS.ChartService.BL.Models.Requests;

public class CalculateForecastRequest
{
    public string SheetName { get; init; }
    public SheetPointInCell? TargetPoint { get; init; }
    public List<SheetPointInCell> SourcePoints { get; init; } = new();
    public List<string> CalculateMethodsIds { get; init; } = new();
    public byte ForecastSpread { get; init; }
}