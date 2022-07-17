using System;

namespace GFS.Portfolio.Api.Models
{
    public class CreatePortfolioRequestDto
    {
        public string Name { get; set; }
        public decimal CashAmount { get; set; }
        public DateTime MomentUtc { get; set; }
    }
}