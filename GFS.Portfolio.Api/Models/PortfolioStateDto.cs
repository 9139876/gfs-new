namespace GFS.Portfolio.Api.Models
{
    public class PortfolioStateDto
    {
        public decimal CashAmount { get; set; }
        public List<AssetModel> Assets { get; set; } = new();
    }
}