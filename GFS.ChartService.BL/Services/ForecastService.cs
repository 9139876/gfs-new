using System.Drawing;
using AutoMapper;
using GFS.AnalysisSystem.Library.Calculation;
using GFS.AnalysisSystem.Library.Calculation.Abstraction;
using GFS.AnalysisSystem.Library.Calculation.Models;
using GFS.ChartService.BL.Models.ProjectViewModel.Sheet.Layers;
using GFS.ChartService.BL.Models.Requests;
using GFS.ChartService.BL.Models.Responses;
using GFS.ChartService.BL.Services.Project;

namespace GFS.ChartService.BL.Services;

public interface IForecastService
{
    List<ForecastMethodsTreeViewModel> GetForecastTree(IForecastTreeGroup forecastTreeGroup);
    List<ForecastPoint> CalculateForecast(CalculateForecastRequest request, Guid clientId);
}

internal class ForecastService : IForecastService
{
    private readonly ISessionService _sessionService;
    private readonly IProjectsCache _projectsCache;
    private readonly IMapper _mapper;

    public ForecastService(
        ISessionService sessionService,
        IProjectsCache projectsCache,
        IMapper mapper)
    {
        _sessionService = sessionService;
        _projectsCache = projectsCache;
        _mapper = mapper;
    }

    public List<ForecastMethodsTreeViewModel> GetForecastTree(IForecastTreeGroup forecastTreeGroup)
    {
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

    public List<ForecastPoint> CalculateForecast(CalculateForecastRequest request, Guid clientId)
    {
        var calculationContext = BuildCalculationContext(request, clientId);
        var calculationResult = ForecastCalculator.Calculate(request.CalculateMethodsIds, calculationContext);

        return _mapper.Map<List<ForecastPoint>>(calculationResult.GetItems);
    }

    private CalculationContext BuildCalculationContext(CalculateForecastRequest request, Guid clientId)
    {
        if (!_sessionService.TryGetProject(clientId, out var projectId))
            throw new InvalidOperationException("У клиента нет активного проекта");

        if (!_projectsCache.TryGetProject(projectId, out var projectModel))
            throw new InvalidOperationException("Проект не найден в кэше");

        var currentSheet = projectModel!.Sheets.SingleOrDefault(s => s.Name == request.SheetName)
                           ?? throw new InvalidOperationException("Текущий лист не найден в проекте");

        var context = new CalculationContext
        {
            TimeFrame = currentSheet.TimeFrame,
            SheetSizeInCells = currentSheet.Size,
            TimeValues = currentSheet.TrackerData.TimeValues,
            PriceValues = currentSheet.TrackerData.PriceValuesDecimal,
            ForecastSpread = request.ForecastSpread,
            PointsFrom = _mapper.Map<Point[]>(request.SourcePoints.Concat(new[] { request.TargetPoint }).Where(p => p != null).ToArray())
        };

        return context;
    }
}