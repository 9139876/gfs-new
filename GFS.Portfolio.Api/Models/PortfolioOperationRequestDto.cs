using GFS.Common.Attributes.Validation;
using GFS.Portfolio.Api.Enums;

namespace GFS.Portfolio.Api.Models
{
    public class PortfolioOperationRequestDto
    {
        public Guid PortfolioId { get; init; }
        
        public PortfolioOperationTypeEnum PortfolioOperationType { get; init; }
        
        [UtcDate]
        public DateTime MomentUtc { get; init; }
        
        public Guid? AssetId { get; init; }
        
        public string? FIGI { get; init; }
        
        [PositiveNumber]
        public int? AssetUnitsCount { get; init; }
        
        public decimal? CashAmount { get; init; }
    }
}