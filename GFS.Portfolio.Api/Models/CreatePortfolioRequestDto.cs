using System.ComponentModel.DataAnnotations;
using GFS.Common.Attributes.Validation;
#pragma warning disable CS8618

namespace GFS.Portfolio.Api.Models
{
    public class CreatePortfolioRequestDto
    {
        [Required]
        public string Name { get; set; }
        
        public decimal CashAmount { get; set; }
        
        [UtcDate]
        public DateTime MomentUtc { get; set; }
    }
}