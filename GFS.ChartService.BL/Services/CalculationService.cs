using GFS.AnalysisSystem.Library.Tendention.Models.BuildTendentionContexts;
using GFS.AnalysisSystem.Library.Tendention.TendentionBuilders;
using GFS.ChartService.BL.Models.ProjectViewModel.Sheet.Layers;
using GFS.ChartService.BL.Models.Requests;
using GFS.ChartService.BL.Services.Project;

namespace GFS.ChartService.BL.Services;

public interface ICalculationService
{
    List<PointForceViewModel> BuildTendention(BuildTendentionRequest request, Guid clientId);
}

internal class CalculationService : ICalculationService
{
    private readonly ISessionService _sessionService;
    private readonly IProjectsCache _projectsCache;

    public CalculationService(
        ISessionService sessionService,
        IProjectsCache projectsCache)
    {
        _sessionService = sessionService;
        _projectsCache = projectsCache;
    }

    public List<PointForceViewModel> BuildTendention(BuildTendentionRequest request, Guid clientId)
    {
        if (!_sessionService.TryGetProject(clientId, out var projectId))
            throw new InvalidOperationException("У клиента нет активного проекта");

        if (!_projectsCache.TryGetProject(projectId, out var projectModel))
            throw new InvalidOperationException("Проект не найден в кэше");

        var currentSheet = projectModel!.Sheets.SingleOrDefault(s => s.Name == request.SheetName)
                           ?? throw new InvalidOperationException("Текущий лист не найден в проекте");

        var tendention = new OutstandingOfNeighborsTendentionBuilder(new BuildOutstandingOfNeighborsTendentionContext {ByNeighborsType = ByNeighborsType.ByThreeEach}, currentSheet.TickerLayerData.Candles).BuildTendention();
        
        // var tendention = new ThreePointsTendentionBuilder(new BuildThreePointsTendentionContext(), currentSheet.TickerLayerData.Candles).BuildTendention();
        
        // var tendention = TendentionBuilder.Build(
        //     new BuildTendentionRequest<BuildThreePointsTendentionContext>(currentSheet.TickerLayerData.Candles, new BuildThreePointsTendentionContext()));

        if (!tendention.TryGetPoints(out var points))
            throw new InvalidOperationException("Построить-то тенденцию удалось, да она некорректная вышла :(");

        // var points = tendention.GetPointsAnyway();
        
        return points.Select(p => new PointForceViewModel
            {
                Index = Guid.NewGuid(),
                IsCorrect = true,
                X = p.Point.X,
                Y = p.Point.Y
            }
        ).ToList();
    }
}