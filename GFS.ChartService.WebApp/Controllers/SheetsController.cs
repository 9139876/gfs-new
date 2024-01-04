using GFS.ChartService.BL.Models.ProjectViewModel.Sheet;
using GFS.ChartService.BL.Models.Requests;
using GFS.ChartService.BL.Services.Project;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace GFS.ChartService.WebApp.Controllers;

[EnableCors("CorsPolicy")]
[Route(nameof(SheetsController))]
public class SheetsController : BaseControllerWithClientIdentifier
{
    private readonly ISheetsService _sheetsService;

    public SheetsController(
        ISheetsService sheetsService)
    {
        _sheetsService = sheetsService;
    }

    [HttpPost(nameof(CreateSheet))]
    public async Task<SheetViewModel> CreateSheet([FromBody] CreateSheetRequest request)
    {
        return await _sheetsService.CreateSheet(request, GetClientId());
    }
}