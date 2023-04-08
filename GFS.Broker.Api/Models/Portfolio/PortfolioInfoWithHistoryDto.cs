namespace GFS.Broker.Api.Models.Portfolio
{
    public class PortfolioInfoWithHistoryDto : PortfolioInfoDto
    {
        public List<PortfolioOperation> History { get; set; } = new();
    }
}