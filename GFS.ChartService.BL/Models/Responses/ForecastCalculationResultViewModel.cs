using GFS.ChartService.BL.Models.ProjectViewModel.Sheet.Layers;

namespace GFS.ChartService.BL.Models.Responses;

public class ForecastCalculationResultViewModel
{
    public List<ForecastPoint> Items { get; init; } = new();
}