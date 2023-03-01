using GFS.Api.Client.Services;
using GFS.Common.Extensions;
using GFS.QuotesService.BackgroundWorker.Api.Interfaces;
using GFS.QuotesService.BackgroundWorker.Api.Models.RequestResponse;
using GFS.QuotesService.Cli.Arguments;
using Spectre.Console.Cli;

namespace GFS.QuotesService.Cli.Commands;

internal class GetTasksCommand : Command<GetTasksCmdArguments>
{
    private readonly IRemoteApiClient _remoteApiClient;

    public GetTasksCommand(IRemoteApiClient remoteApiClient)
    {
        _remoteApiClient = remoteApiClient;
    }

    public override int Execute(CommandContext context, GetTasksCmdArguments settings)
    {
        var apiResponse = _remoteApiClient.Call<GetTasks, GetTasksRequest, GetTasksResponse>(new GetTasksRequest { TaskStates = settings.TaskStates }).Result;
        
        Console.WriteLine(apiResponse.Tasks.Serialize());

        return 0;
    }
}