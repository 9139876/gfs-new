using GFS.Api.Services;
using GFS.TradingStrategyTester.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GFS.TradingStrategyTester.Api.Interfaces;

[Route(nameof(CreateTradingStrategyTestingItem))]
public abstract class CreateTradingStrategyTestingItem : ApiServiceWithRequestResponse<TradingStrategyTestingItemContext, Guid>
{
    protected CreateTradingStrategyTestingItem(ILogger<CreateTradingStrategyTestingItem> logger) : base(logger)
    {
    }
}