namespace GFS.GrailCommon.Models;

public class VolumeTicksQuoteModel
{
    public VolumeTicksQuoteModel(decimal priceValue, DateTime start, DateTime end)
    {
        PriceValue = priceValue;
        Start = start;
        End = end;
    }

    public decimal PriceValue { get; }

    public DateTime Start { get; }

    public DateTime End { get; }

    public TimeSpan Duration => End - Start;
}