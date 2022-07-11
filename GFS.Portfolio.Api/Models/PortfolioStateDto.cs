using System.Collections.Generic;

namespace GFS.Portfolio.Api.Models
{
    public class PortfolioStateDto
    {
        public decimal CashAmount { get; set; }
        public List<(AssetModel, int)> Assets { get; set; }
    }
}