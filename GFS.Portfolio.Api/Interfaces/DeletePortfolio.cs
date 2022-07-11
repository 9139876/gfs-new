using GFS.Api.Services;
using GFS.Portfolio.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GFS.Portfolio.Api.Interfaces
{
    [Route(nameof(DeletePortfolio))]
    public abstract class DeletePortfolio : ApiServiceWithRequest<DeletePortfolioRequestDto>
    {
        protected DeletePortfolio(ILogger logger) : base(logger)
        {
        }
    }
}