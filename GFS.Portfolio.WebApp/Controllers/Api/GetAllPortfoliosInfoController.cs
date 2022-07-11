using System.Collections.Generic;
using System.Threading.Tasks;
using GFS.Portfolio.Api.Interfaces;
using GFS.Portfolio.Api.Models;
using Microsoft.Extensions.Logging;

namespace GFS.Portfolio.WebApp.Controllers.Api
{
    public class GetAllPortfoliosInfoController : GetAllPortfoliosInfo
    {
        public GetAllPortfoliosInfoController(ILogger logger) : base(logger)
        {
        }

        protected override Task<List<PortfolioInfoDto>> ExecuteInternal()
        {
            throw new System.NotImplementedException();
        }
    }
}