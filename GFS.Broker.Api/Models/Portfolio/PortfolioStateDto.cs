namespace GFS.Broker.Api.Models.Portfolio
{
    public class PortfolioStateDto
    {
        public decimal CashAmount { get; set; }
        public List<AssetModel> Assets { get; set; } = new();
    }
}