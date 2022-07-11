using System.Threading.Tasks;
using GFS.Portfolio.Api.Interfaces;
using GFS.Portfolio.Api.Models;
using Microsoft.Extensions.Logging;

namespace GFS.Portfolio.WebApp.Controllers.Api
{
    public class CreatePortfolioController : CreatePortfolio
    {
        public CreatePortfolioController(ILogger logger) : base(logger)
        {
        }

        protected override Task<PortfolioInfoDto> ExecuteInternal(CreatePortfolioRequestDto request)
        {
            throw new System.NotImplementedException();
        }
    }
}