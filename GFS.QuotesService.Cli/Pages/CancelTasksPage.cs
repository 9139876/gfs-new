using GFS.Api.Client.Services;
using GFS.Common.Models;
using GFS.ConsoleLibrary.Menu;
using GFS.QuotesService.BackgroundWorker.Api.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace GFS.QuotesService.Cli.Pages;

public class CancelTasksPage : ConsolePage
{
    public CancelTasksPage(ConsolePage? parent) : base(parent)
    {
    }

    protected override void ShowInternal()
    {
        var response = string.Empty;

        if (!DoWithSpinner(() =>
            {
                var remoteApiClient = Program.ServiceProvider.GetRequiredService<IRemoteApiClient>();
                var apiResponse = remoteApiClient.Call<CancelTasks, StandardResponse>().Result;
                response = apiResponse.IsSuccess ? "Успешно выполнено" : $"Произошла ошибка - {apiResponse.ErrorMessage}";
            })) return;

        Console.WriteLine(response);
        AnyKeyAndReturnTo(Root);
    }
}