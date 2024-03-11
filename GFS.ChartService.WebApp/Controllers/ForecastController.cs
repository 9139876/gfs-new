using GFS.AnalysisSystem.Library.Calculation;
using GFS.AnalysisSystem.Library.Calculation.Abstraction;
using GFS.ChartService.BL.Models.Responses;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace GFS.ChartService.WebApp.Controllers;

[EnableCors("CorsPolicy")]
[Route(nameof(ForecastController))]
public class ForecastController : BaseControllerWithClientIdentifier
{
    [HttpGet(nameof(GetForecastMethodsTree))]
    public List<ForecastMethodsTreeViewModel> GetForecastMethodsTree()
    {
        return GetForecastTree(ForecastCalculator.ForecastTree);
    }

    private static List<ForecastMethodsTreeViewModel> GetForecastTree(IForecastTreeGroup forecastTreeGroup)
    {
        var result = new List<ForecastMethodsTreeViewModel>();

        foreach (var item in forecastTreeGroup.Children)
        {
            if (item is IForecastTreeGroup @group)
                result.Add(new ForecastMethodsTreeViewModel
                {
                    Key = item.Identifier,
                    Title = item.Name,
                    Children = GetForecastTree(@group).ToArray()
                });
            else
            {
                result.Add(new ForecastMethodsTreeViewModel
                {
                    Key = item.Identifier,
                    Title = item.Name
                });
            }
        }

        return result;
    }
}