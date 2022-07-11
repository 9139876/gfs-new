using System.Threading.Tasks;
using GFS.Portfolio.Api.Interfaces;
using GFS.Portfolio.Api.Models;
using Microsoft.Extensions.Logging;

namespace GFS.Portfolio.WebApp.Controllers.Api
{
    public class GetPortfolioInfoWithHistoryController : GetPortfolioInfoWithHistory
    {
        public GetPortfolioInfoWithHistoryController(ILogger logger) : base(logger)
        {
        }

        protected override Task<PortfolioInfoWithHistoryDto> ExecuteInternal(GetPortfolioInfoRequestDto request)
        {
            throw new System.NotImplementedException();
        }
    }
}