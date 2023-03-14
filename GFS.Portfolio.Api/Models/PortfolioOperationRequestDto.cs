using GFS.Portfolio.Api.Enums;

namespace GFS.Portfolio.Api.Models
{
    public class PortfolioOperationRequestDto
    {
        public Guid PortfolioId { get; init; }
        public PortfolioOperationTypeEnum PortfolioOperationType { get; init; }
        public DateTime MomentUtc { get; init; }
        public Guid? AssetId { get; init; }
        public string? FIGI { get; init; }
        public int? AssetUnitsCount { get; init; }
        public decimal? CashAmount { get; init; }
    }
}