using System.Diagnostics.CodeAnalysis;

namespace GFS.WebApplication.Models;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class EnvResponse
{
    public string? ApplicationName { get; init; }
    public string? EnvironmentName { get; init; }
    public string? UserName { get; init; }
    public string? MachineName { get; init; }
    public string? OsVersion { get; init; }
}