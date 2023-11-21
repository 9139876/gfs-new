#pragma warning disable CS8618
namespace GFS.ChartService.BL.Models.Responses;

public class ProjectInfoViewModel
{
    public Guid Id { get; init; }
    public string ProjectName { get; init; }
    
    public DateTime CreatedDate { get; init; }

    public DateTime ModificationDate { get; init; }
}