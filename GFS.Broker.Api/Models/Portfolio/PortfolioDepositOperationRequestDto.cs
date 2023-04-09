using GFS.Broker.Api.Enums;
using GFS.Common.Attributes.Validation;

namespace GFS.Broker.Api.Models.Portfolio
{
    public class PortfolioDepositOperationRequestDto
    {
        public Guid PortfolioId { get; init; }

        public PortfolioDepositOperationType PortfolioOperationType { get; init; }

        [UtcDate] 
        public DateTime MomentUtc { get; init; }

        public decimal? CashAmount { get; init; }
    }
}