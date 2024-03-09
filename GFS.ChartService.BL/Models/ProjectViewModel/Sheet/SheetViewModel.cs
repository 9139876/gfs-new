using System.Drawing;
using GFS.ChartService.BL.Models.ProjectViewModel.Sheet.Layers;

namespace GFS.ChartService.BL.Models.ProjectViewModel.Sheet;

public class SheetViewModel
{
    public string Name { get; init; }

    // public SheetInfoExtViewModel SheetMetaInfo { get; init; }

    public Size Size { get; init; }

    public TrackerDataViewModel TrackerData { get; init; }

    public TickerLayerDataViewModel TickerLayerData { get; init; }

    public GridLayerDataViewModel GridLayerData { get; init; }

    public PfLayerDataViewModel PfLayerData { get; init; }

    public PointerLayerDataViewModel PointerLayerData { get; init; }

    public ForecastLayerDataViewModel ForecastLayerData { get; init; }
}