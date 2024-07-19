namespace GFS.AnalysisSystem.Library.Internal.TimeConverter;

internal class TimeConverterResultItem
{
    public TimeConverterResultItem(TimeSpan value, string description)
    {
        Value = value;
        Description = description;
    }

    public TimeSpan Value { get; }
    public string Description { get; }
}