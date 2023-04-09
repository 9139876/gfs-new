using System.ComponentModel.DataAnnotations;

#pragma warning disable CS8618

namespace GFS.Broker.Api.Models.Portfolio
{
    public class CreatePortfolioRequestDto
    {
        [Required]
        public string Name { get; init; }
        
        public string Description { get; init; }
    }
}