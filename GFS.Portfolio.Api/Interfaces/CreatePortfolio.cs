using GFS.Api.Services;
using GFS.Portfolio.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GFS.Portfolio.Api.Interfaces;

[Route(nameof(CreatePortfolio))]
public abstract class CreatePortfolio : ApiServiceWithRequestResponse<CreatePortfolioRequestDto, PortfolioInfoDto>
{
    protected CreatePortfolio(ILogger logger) : base(logger)
    {
    }
}