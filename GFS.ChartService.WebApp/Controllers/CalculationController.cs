using GFS.ChartService.BL.Models.ProjectViewModel.Sheet.Layers;
using GFS.ChartService.BL.Models.Requests;
using GFS.ChartService.BL.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace GFS.ChartService.WebApp.Controllers;

[EnableCors("CorsPolicy")]
[Route(nameof(CalculationController))]
public class CalculationController : BaseControllerWithClientIdentifier
{
    private readonly ICalculationService _calculationService;

    public CalculationController(
        ICalculationService calculationService)
    {
        _calculationService = calculationService;
    }

    [HttpPost(nameof(BuildTendention))]
    public List<PointForceViewModel> BuildTendention([FromBody] BuildTendentionRequest request)
    {
        return _calculationService.BuildTendention(request, GetClientId());
    }
}