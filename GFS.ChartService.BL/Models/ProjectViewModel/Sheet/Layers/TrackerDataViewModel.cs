// ReSharper disable UnusedAutoPropertyAccessor.Global

#pragma warning disable CS8618
namespace GFS.ChartService.BL.Models.ProjectViewModel.Sheet.Layers;

public class TrackerDataViewModel
{
    public DateTime[] TimeValues { get; init; }

    public string[] PriceValues { get; init; }

    public decimal[] PriceValuesDecimal { get; init; }
}