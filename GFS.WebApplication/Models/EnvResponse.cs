namespace GFS.WebApplication.Models;

public class EnvResponse
{
    public string ApplicationName { get; init; }
    public string EnvironmentName { get; init; }
    public string UserName { get; init; }
    public string MachineName { get; init; }
    public string OSVersion { get; init; }
}