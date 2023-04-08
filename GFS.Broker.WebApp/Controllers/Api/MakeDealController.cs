using GFS.Broker.Api.Interfaces;
using GFS.Broker.Api.Models;
using GFS.Broker.BL.Services;

namespace GFS.Broker.WebApp.Controllers.Api;

public class MakeDealController : MakeDeal
{
    private readonly IDealService _dealService;

    public MakeDealController(
        ILogger logger,
        IDealService dealService) : base(logger)
    {
        _dealService = dealService;
    }

    protected override async Task<MakeDealResponse> ExecuteInternal(MakeDealRequest request)
    {
        return await _dealService.MakeDeal(request);
    }
}