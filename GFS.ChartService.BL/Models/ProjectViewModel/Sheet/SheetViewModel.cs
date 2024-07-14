using System.Drawing;
using GFS.ChartService.BL.Models.ProjectViewModel.Sheet.Layers;
using GFS.GrailCommon.Enums;
#pragma warning disable CS8618

namespace GFS.ChartService.BL.Models.ProjectViewModel.Sheet;

public class SheetViewModel
{
    public string? Name { get; init; }

    public TimeFrameEnum TimeFrame { get; init; }

    // public SheetInfoExtViewModel SheetMetaInfo { get; init; }

    public Size Size { get; init; }

    public TrackerDataViewModel TrackerData { get; init; }

    public TickerLayerDataViewModel TickerLayerData { get; init; }

    public GridLayerDataViewModel GridLayerData { get; init; }

    public PfLayerDataViewModel PfLayerData { get; init; }

    public PointerLayerDataViewModel PointerLayerData { get; init; }

    public ForecastLayerDataViewModel ForecastLayerData { get; init; }
}