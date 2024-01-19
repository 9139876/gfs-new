namespace GFS.ChartService.BL.Models.ProjectViewModel.Sheet.Layers;

public class TickerLayerDataViewModel
{
    public List<CandleInCells> Candles { get; init; } = new();

    public TickerLayerProperties Properties { get; init; } = new();
}

public class CandleInCells
{
    public int Date { get; init; }

    public int Open { get; init; }

    public int High { get; init; }

    public int Low { get; init; }

    public int Close { get; init; }
}

public class TickerLayerProperties
{
    public bool IsVisible { get; init; } = true;
    public string Color { get; init; } = "Red";
    public CandleType CandleType { get; init; } = CandleType.Gann;
}

public enum CandleType
{
    Gann = 1,
    Japan = 2,
    OHLC = 3,
}