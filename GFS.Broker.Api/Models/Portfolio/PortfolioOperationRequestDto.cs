using GFS.Broker.Api.Enums;
using GFS.Common.Attributes.Validation;

namespace GFS.Broker.Api.Models.Portfolio
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