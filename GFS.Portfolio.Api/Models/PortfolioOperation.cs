using System;
using GFS.GrailCommon.Models;
using GFS.Portfolio.Api.Enums;

namespace GFS.Portfolio.Api.Models
{
    public class PortfolioOperation
    {
        public DateTime MomentUtc { get; set; }
        public OperationTypeEnum OperationType { get; set; }
        public AssetIdentifier? AssetIdentifier { get; set; }
        public int? AssetsLotsChange { get; set; }
        public decimal? AssetDealPrice { get; set; }
        public decimal CashChange { get; set; }
        public PortfolioStateDto PortfolioStateAfterOperation { get; set; }
    }
}