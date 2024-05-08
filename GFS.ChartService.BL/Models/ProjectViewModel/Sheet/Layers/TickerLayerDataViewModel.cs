using GFS.GrailCommon.Models;

namespace GFS.ChartService.BL.Models.ProjectViewModel.Sheet.Layers;

public class TickerLayerDataViewModel
{
    public List<CandleInCells> Candles { get; init; } = new();

    public TickerLayerProperties Properties { get; init; } = new();
}

public class TickerLayerProperties
{
    public bool IsVisible { get; init; } = true;
    public string Color { get; init; } = "White";
    public CandleType CandleType { get; init; } = CandleType.Gann;
}

public enum CandleType
{
    Gann = 1,
    Japan = 2,
    OHLC = 3,
}