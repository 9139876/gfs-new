using GFS.Api.Services;
using GFS.Broker.Api.Models.TestDealer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GFS.Broker.Api.Interfaces;

[Route(nameof(TryPerformPendingOrders))]
public abstract class TryPerformPendingOrders : ApiServiceWithRequestResponse<TryPerformPendingOrdersRequest, TryPerformPendingOrdersResponse>
{
    protected TryPerformPendingOrders(ILogger<TryPerformPendingOrders> logger) : base(logger)
    {
    }
}