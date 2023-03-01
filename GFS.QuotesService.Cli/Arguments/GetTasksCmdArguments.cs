using GFS.BkgWorker.Enum;
using Spectre.Console.Cli;

namespace GFS.QuotesService.Cli.Arguments;

internal class GetTasksCmdArguments : CommandSettings
{
    public List<TaskStateEnum> TaskStates { get; init; } = new();
}