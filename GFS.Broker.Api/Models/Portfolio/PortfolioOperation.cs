using System.ComponentModel.DataAnnotations;
using GFS.Common.Attributes.Validation;

#pragma warning disable CS8618

namespace GFS.Broker.Api.Models.Portfolio
{
    public class PortfolioOperation
    {
        [UtcDate]
        public DateTime MomentUtc { get; set; }
        
        // public PortfolioOperationTypeEnum PortfolioOperationType { get; set; }
        
        public Guid? AssetId { get; set; }
        
        public int? AssetsLotsChange { get; set; }
        
        public decimal? AssetDealPrice { get; set; }
        
        public decimal CashChange { get; set; }
        
        [Required]
        public PortfolioStateDto PortfolioStateAfterOperation { get; set; }
    }
}