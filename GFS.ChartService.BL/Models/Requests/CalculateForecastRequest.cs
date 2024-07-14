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
    public ForecastWindowDto ForecastWindow { get; init; }
}

public class ForecastWindowDto
{
    public int X1 { get; init; }
    public int Y1 { get; init; }
    public int X2 { get; init; }
    public int Y2 { get; init; }
}