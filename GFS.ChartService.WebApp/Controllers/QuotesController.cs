using GFS.ChartService.BL.Services;
using GFS.Common.Models;
using GFS.QuotesService.Api.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace GFS.ChartService.WebApp.Controllers;

[EnableCors("CorsPolicy")]
[Route(nameof(QuotesController))]
public class QuotesController : ControllerBase
{
    private readonly IQuotesService _quotesService;

    public QuotesController(
        IQuotesService quotesService)
    {
        _quotesService = quotesService;
    }

    [HttpPost(nameof(GetAssetsInfo))]
    public async Task<WebAppResponseContainer<List<AssetsInfoDto>>> GetAssetsInfo([FromBody]AssetsFilter request)
    {
        var result = await _quotesService.GetAssetsInfo(request);
        return WebAppResponseContainer<List<AssetsInfoDto>>.GetSuccessResponse(result); 
    } 
}