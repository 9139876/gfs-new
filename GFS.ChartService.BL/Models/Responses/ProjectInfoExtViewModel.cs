namespace GFS.ChartService.BL.Models.Responses;

public class ProjectInfoExtViewModel
{
    public List<SheetInfoExtViewModel> SheetsInfos { get; init; } = new();
}

// Инфо о листах (актив, тф, первая-последняя дата), инфо о кошельках, ???

public record SheetInfoExtViewModel(string AssetName, string TimeFrame, DateTime FirstDate, DateTime LastDate);