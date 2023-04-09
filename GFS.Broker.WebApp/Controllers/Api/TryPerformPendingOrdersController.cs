using GFS.Broker.Api.Interfaces;
using GFS.Broker.Api.Models.TestDealer;
using GFS.Broker.BL.Services;

namespace GFS.Broker.WebApp.Controllers.Api;

public class TryPerformPendingOrdersController : TryPerformPendingOrders
{
    private readonly IDealService _dealService;

    public TryPerformPendingOrdersController(
        ILogger logger,
        IDealService dealService) : base(logger)
    {
        _dealService = dealService;
    }

    protected override async Task<TryPerformPendingOrdersResponse> ExecuteInternal(TryPerformPendingOrdersRequest request)
    {
        return await _dealService.TryPerformPendingOrders(request);
    }
}