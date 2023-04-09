using System.ComponentModel.DataAnnotations;

#pragma warning disable CS8618

namespace GFS.Broker.Api.Models.Portfolio
{
    public class PortfolioInfoDto
    {
        public Guid PortfolioId { get; init; }
        
        [Required]
        public string Name { get; init; }
        
        [Required]
        public PortfolioStateDto PortfolioState { get; init; }

        [Required]
        public List<PendingOrderDto> PendingOrders { get; init; } = new();
    }
}