using GFS.Api.Client.Services;
using GFS.Common.Models;
using GFS.ConsoleLibrary.Menu;
using GFS.QuotesService.BackgroundWorker.Api.Interfaces;
using GFS.QuotesService.BackgroundWorker.Api.Models.RequestResponse;
using GFS.QuotesService.Cli.Models;
using Microsoft.Extensions.DependencyInjection;

namespace GFS.QuotesService.Cli.Pages;

public class AddTasksExecutePage : ConsolePage
{
    private readonly AddTasksRequestBillet _requestBillet;

    public AddTasksExecutePage(
        ConsolePage? parent,
        AddTasksRequestBillet requestBillet) : base(parent)
    {
        _requestBillet = requestBillet;
    }

    protected override void ShowInternal()
    {
        var request = _requestBillet.CreateRequest();
        
        if (request.Tasks.Any() && request.Tasks.All(t => t.IsValid()))
        {
            var response = string.Empty;

            if (!DoWithSpinner(() =>
                {
                    var remoteApiClient = Program.ServiceProvider.GetRequiredService<IRemoteApiClient>();
                    var apiResponse = remoteApiClient.Call<AddTasks, AddTasksRequest, StandardResponse>(request).Result;
                    response = apiResponse.IsSuccess ? "Успешно выполнено" : $"Произошла ошибка - {apiResponse.ErrorMessage}";
                })) return;

            Console.WriteLine(response);
            AnyKeyAndReturnTo(Root);
        }
        else
        {
            Console.WriteLine($"Запрос составлен некорректно");
            AnyKeyAndReturnTo(Parent!);
        }
    }
}