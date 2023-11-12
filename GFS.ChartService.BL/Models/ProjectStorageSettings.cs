#pragma warning disable CS8618
namespace GFS.ChartService.BL.Models;

public class ProjectStorageSettings
{
    public int MinimumSupportedProjectVersion { get; init; }

    public string ProjectsStoragePath { get; init; }
    
    public int MaxInactiveAgeInSeconds { get; init; }
}