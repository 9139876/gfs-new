using AutoMapper;
using GFS.QuotesService.Api.Interfaces;
using GFS.QuotesService.Api.Models;
using GFS.QuotesService.BL.Services;

namespace GFS.QuotesService.WebApp.Controllers.Api;

public class GetAssetQuotesInfoController : GetAssetQuotesInfo
{
    private readonly IGetDataService _getDataService;

    public GetAssetQuotesInfoController(
        ILogger logger,
        IGetDataService getDataService) : base(logger)
    {
        _getDataService = getDataService;
    }

    protected override async Task<QuotesInfoDto> ExecuteInternal(Guid request)
    {
        return await _getDataService.GetAssetQuotesInfo(request);
    }
}