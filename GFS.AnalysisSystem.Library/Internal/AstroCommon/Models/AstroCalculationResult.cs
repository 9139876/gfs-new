using GFS.AnalysisSystem.Library.Internal.AstroCommon.Enums;

namespace GFS.AnalysisSystem.Library.Internal.AstroCommon.Models
{
    internal class AstroCalculationResult
    {
        public bool IsSuccess { get; init; }
        public DateTime Date { get; init; }
        public Dictionary<PlanetParameterType, double>? Result { get; init; }
        public string? Error { get; init; } 
    }
}