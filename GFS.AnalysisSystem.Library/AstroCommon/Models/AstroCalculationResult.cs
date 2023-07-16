namespace GFS.AnalysisSystem.Library.AstroCommon.Models
{
    public class AstroCalculationResult
    {
        public bool IsSuccess { get; set; }
        public double Longitude { get; init; }
        public string? Error { get; init; } 
    }
}