#pragma warning disable CS8618
namespace GFS.ChartService.BL.Models;

public class ProjectInfoViewModel
{
    public string ProjectName { get; init; }
    
    public DateTime CreatedDate { get; init; }

    public DateTime ModificationDate { get; init; }
}