namespace GFS.BalanceCalculation.BL.Auxiliary.Astro.Models
{
    public class AstroCalculationResult
    {
        public bool IsSuccess { get; set; }
        public double Longitude { get; init; }
        public string? Error { get; init; } 
    }
}