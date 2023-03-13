using GFS.Portfolio.Api.Enums;

namespace GFS.Portfolio.Api.Models
{
    public class OperationRequestDto
    {
        public Guid PortfolioId { get; set; }
        public PortfolioOperationTypeEnum PortfolioOperationType { get; set; }
        public Guid? AssetId { get; set; }
        public int? AssetLotsCount { get; set; }
        public decimal? AssetDealPrice { get; set; }
        public decimal? CashAmount { get; set; }
        public DateTime MomentUtc { get; set; }
    }
}