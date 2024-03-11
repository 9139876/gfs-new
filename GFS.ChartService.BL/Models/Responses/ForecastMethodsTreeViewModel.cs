#pragma warning disable CS8618
namespace GFS.ChartService.BL.Models.Responses;

public class ForecastMethodsTreeViewModel
{
    public string Title { get; init; }
    public string Key { get; init; }
    public ForecastMethodsTreeViewModel[] Children { get; init; }
}