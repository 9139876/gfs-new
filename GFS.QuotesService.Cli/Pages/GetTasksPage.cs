using GFS.Api.Client.Services;
using GFS.BackgroundWorker.Enums;
using GFS.Common.Extensions;
using GFS.ConsoleLibrary.Menu;
using GFS.QuotesService.BackgroundWorker.Api.Interfaces;
using GFS.QuotesService.BackgroundWorker.Api.Models.RequestResponse;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

namespace GFS.QuotesService.Cli.Pages;

public class GetTasksPage : ConsolePage
{
    private readonly Dictionary<string, Action> _items;

    private TaskStateEnum? _taskState;

    public GetTasksPage(ConsolePage? parent) : base(parent)
    {
        _items = new Dictionary<string, Action>
        {
            { "Ожидает выполнения", () => _taskState = TaskStateEnum.PendingExecution },
            { "Выполняется", () => _taskState = TaskStateEnum.Executing },
            { "Выполнена", () => _taskState = TaskStateEnum.Completed },
            { "Прервана из-за ошибки", () => _taskState = TaskStateEnum.Failed },
            { "Отменена", () => _taskState = TaskStateEnum.Canceled },
            { "Все", () => _taskState = null },
            { "Назад", () => { } }
        };
    }

    protected override void ShowInternal()
    {
        var itemName = AnsiConsole.Prompt(GetSelectionPrompt("Получение списка задач. Статус задач:", _items.Keys));

        if (itemName == "Назад")
            Parent?.Show();
        else
        {
            _items[itemName].Invoke();

            var response = string.Empty;

            if (!DoWithSpinner(() =>
                {
                    var apiRequest = _taskState.HasValue
                        ? new GetTasksRequest { TaskStates = new List<TaskStateEnum> { _taskState.Value } }
                        : new GetTasksRequest();

                    var remoteApiClient = Program.ServiceProvider.GetRequiredService<IRemoteApiClient>();

                    var apiResponse = remoteApiClient.Call<GetTasks, GetTasksRequest, GetTasksResponse>(apiRequest).Result;
                    response = apiResponse.Tasks.Serialize(jsonSerializerSettings => jsonSerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss");
                })) return;

            Console.WriteLine(response);
            AnyKeyAndReturnTo(Root);
        }
    }
}