using GFS.ChartService.BL.Models.ProjectViewModel.Sheet;

namespace GFS.ChartService.BL.Models.ProjectViewModel;

public class ProjectViewModel
{
    public string? ProjectName { get; init; }

    public List<SheetViewModel> Sheets { get; init; } = new();
}