namespace GFS.Portfolio.Api.Models
{
    public class PortfolioInfoWithHistoryDto : PortfolioInfoDto
    {
        public List<PortfolioOperation> History { get; set; } = new();
    }
}