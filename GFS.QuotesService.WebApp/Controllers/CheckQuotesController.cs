using GFS.QuotesService.Api.Models.CheckQuotes;
using GFS.QuotesService.BL.Services;
using Microsoft.AspNetCore.Mvc;

namespace GFS.QuotesService.WebApp.Controllers;

public class CheckQuotesController : ControllerBase
{
    private readonly ICheckQuotesService _checkQuotesService;

    public CheckQuotesController(ICheckQuotesService checkQuotesService)
    {
        _checkQuotesService = checkQuotesService;
    }

    [HttpPost(nameof(CompareTimeframesCheckQuotes))]
    public async Task<CompareTimeframesCheckQuotesResponse> CompareTimeframesCheckQuotes([FromBody] CompareTimeframesCheckQuotesRequest request)
    {
        return await _checkQuotesService.CompareTimeframesCheckQuotes(request);
    }
}