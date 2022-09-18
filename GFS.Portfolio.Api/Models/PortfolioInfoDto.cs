namespace GFS.Portfolio.Api.Models
{
    public class PortfolioInfoDto
    {
        public Guid PortfolioId { get; set; }
        public string Name { get; set; }
        public PortfolioStateDto PortfolioState { get; set; }
    }
}