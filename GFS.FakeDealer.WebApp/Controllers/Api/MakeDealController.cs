using GFS.FakeDealer.Api.Interfaces;
using GFS.FakeDealer.Api.Models;
using GFS.FakeDealer.BL.Services;

namespace GFS.FakeDealer.WebApp.Controllers.Api;

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