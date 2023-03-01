using GFS.Api.Client.Services;
using GFS.Common.Extensions;
using GFS.QuotesService.BackgroundWorker.Api.Interfaces;
using GFS.QuotesService.BackgroundWorker.Api.Models.RequestResponse;
using GFS.QuotesService.Cli.Arguments;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace GFS.QuotesService.Cli.Commands;

internal class GetTasksCommand : Command<GetTasksCmdArguments>
{
    public override int Execute(CommandContext context, GetTasksCmdArguments settings)
    {
        var remoteApiClient = Program.ServiceProvider.GetRequiredService<IRemoteApiClient>();

        var task = remoteApiClient.Call<GetTasks, GetTasksRequest, GetTasksResponse>(new GetTasksRequest { TaskStates = settings.TaskStates });

        task.Wait();
        
        
        var apiResponse = remoteApiClient.Call<GetTasks, GetTasksRequest, GetTasksResponse>(new GetTasksRequest { TaskStates = settings.TaskStates }).Result;
        
        Console.WriteLine(apiResponse.Tasks.Serialize());

        return 0;
    }
}