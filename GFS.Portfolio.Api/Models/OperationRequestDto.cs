using System;
using GFS.GrailCommon.Models;
using GFS.Portfolio.Api.Enums;

namespace GFS.Portfolio.Api.Models
{
    public class OperationRequestDto
    {
        public Guid PortfolioId { get; set; }
        public OperationTypeEnum OperationType { get; set; }
        public AssetIdentifier? AssetIdentifier { get; set; }
        public int? AssetLotsCount { get; set; }
        public decimal? AssetDealPrice { get; set; }
        public decimal? CashAmount { get; set; }
        public DateTime MomentUtc { get; set; }
    }
}