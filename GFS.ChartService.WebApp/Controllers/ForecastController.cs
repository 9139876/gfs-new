using GFS.AnalysisSystem.Library.Calculation;
using GFS.ChartService.BL.Models.Requests;
using GFS.ChartService.BL.Models.Responses;
using GFS.ChartService.BL.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace GFS.ChartService.WebApp.Controllers;

[EnableCors("CorsPolicy")]
[Route(nameof(ForecastController))]
public class ForecastController : BaseControllerWithClientIdentifier
{
    private readonly IForecastService _forecastService;

    public ForecastController(
        IForecastService forecastService)
    {
        _forecastService = forecastService;
    }

    [HttpGet(nameof(GetForecastMethodsTree))]
    public List<ForecastMethodsTreeViewModel> GetForecastMethodsTree()
    {
        return _forecastService.GetForecastTree(ForecastCalculator.ForecastTree);
    }

    [HttpPost(nameof(CalculateForecast))]
    public ForecastCalculationResultViewModel CalculateForecast([FromBody] CalculateForecastRequest request)
    {
        return _forecastService.CalculateForecast(request, GetClientId());
    }
}