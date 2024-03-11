namespace GFS.AnalysisSystem.Library.Internal.AstroCommon.Models;

internal class AstroParameterCalculationResult
{
    public bool IsSuccess { get; init; }
    public DateTime Date { get; init; }
    public double Result { get; init; }
    public string? Error { get; init; }
}