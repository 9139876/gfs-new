using GFS.QuotesService.Api.Interfaces;
using GFS.QuotesService.Api.Models;
using GFS.QuotesService.BL.Services;

namespace GFS.QuotesService.WebApp.Controllers.Api;

public class GetQuotesController : GetQuotes 
{
    private readonly IGetDataService _getDataService;

    public GetQuotesController(
        ILogger logger,
        IGetDataService getDataService) : base(logger)
    {
        _getDataService = getDataService;
    }

    protected override async Task<GetQuotesResponse> ExecuteInternal(GetQuotesRequest request)
    {
        return await _getDataService.GetQuotes(request);
    }
}