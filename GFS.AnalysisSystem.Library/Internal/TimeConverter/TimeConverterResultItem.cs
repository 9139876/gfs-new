namespace GFS.AnalysisSystem.Library.Internal.TimeConverter;

internal class TimeConverterResultItem
{
    public TimeConverterResultItem(ushort value, string description)
    {
        if (value is < 1 or > 999)
            throw new InvalidOperationException($"Значение времени должно быть в диапазоне от 1 до 999, а пришло {value}");

        Value = value;
        Description = description;
    }

    public ushort Value { get; }
    public string Description { get; }
}