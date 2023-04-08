using GFS.Api.Services;
using GFS.Broker.Api.Models.Portfolio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GFS.Broker.Api.Interfaces.Portfolio
{
    [Route(nameof(DeletePortfolio))]
    public abstract class DeletePortfolio : ApiServiceWithRequest<DeletePortfolioRequestDto>
    {
        protected DeletePortfolio(ILogger logger) : base(logger)
        {
        }
    }
}