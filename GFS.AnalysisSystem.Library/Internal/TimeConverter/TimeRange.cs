namespace GFS.AnalysisSystem.Library.Internal.TimeConverter;

public class TimeRange
{
    public TimeRange(TimeSpan value1, TimeSpan value2)
    {
        if (value1.TotalMilliseconds <= 0 || value2.TotalMilliseconds <= 0)
            throw new InvalidOperationException("Значение границы диапазона должно иметь положительное значение продолжительности");
        
        MinValue = value1 < value2 ? value1 : value2;
        MaxValue = value1 > value2 ? value1 : value2;
    }

    public TimeSpan MinValue { get; }
    public TimeSpan MaxValue { get; }

    public bool IsZeroLength => MinValue == MaxValue;
}