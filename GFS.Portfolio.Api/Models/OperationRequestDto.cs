using System;
using GFS.Portfolio.Api.Enums;

namespace GFS.Portfolio.Api.Models
{
    public class OperationRequestDto
    {
        public Guid PortfolioId { get; set; }
        public OperationTypeEnum OperationType { get; set; }
        public Guid? AssetId { get; set; }
        public int? AssetsCount { get; set; }
        public decimal? AssetDealPrice { get; set; }
        public decimal CashAmount { get; set; }
        public DateTime MomentUtc { get; set; }
    }
}