using System.Threading.Tasks;
using GFS.Portfolio.Api.Interfaces;
using GFS.Portfolio.Api.Models;
using Microsoft.Extensions.Logging;

namespace GFS.Portfolio.WebApp.Controllers.Api
{
    public class DeletePortfolioController : DeletePortfolio
    {
        public DeletePortfolioController(ILogger logger) : base(logger)
        {
        }

        protected override Task ExecuteInternal(DeletePortfolioRequestDto request)
        {
            throw new System.NotImplementedException();
        }
    }
}