using GFS.ChartService.BL.Services;
using GFS.QuotesService.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace GFS.ChartService.WebApp.Controllers;

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
    public async Task<List<AssetsInfoDto>> GetAssetsInfo([FromBody]AssetsFilter request)
    {
        return await _quotesService.GetAssetsInfo(request);
    } 
}