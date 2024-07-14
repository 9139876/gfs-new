using GFS.ChartService.BL.Models.ProjectViewModel.Sheet;
using GFS.GrailCommon.Models;
#pragma warning disable CS8618

namespace GFS.ChartService.BL.Models.Requests;

public class CalculateForecastRequest
{
    public string SheetName { get; init; }
    public PriceTimePointInCells? TargetPoint { get; init; }
    public List<PriceTimePointInCells> SourcePoints { get; init; } = new();
    public List<string> CalculateMethodsIds { get; init; } = new();
    public byte ForecastSpread { get; init; }
}