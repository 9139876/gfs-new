using GFS.Api.Services;
using GFS.Broker.Api.Models.Portfolio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GFS.Broker.Api.Interfaces.Portfolio;

[Route(nameof(CreatePortfolio))]
public abstract class CreatePortfolio : ApiServiceWithRequestResponse<CreatePortfolioRequestDto, PortfolioInfoDto>
{
    protected CreatePortfolio(ILogger logger) : base(logger)
    {
    }
}