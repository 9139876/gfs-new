using GFS.ConsoleLibrary.Menu;
using GFS.QuotesService.BackgroundWorker.Api.Enum;
using GFS.QuotesService.Cli.Models;
using Spectre.Console;

namespace GFS.QuotesService.Cli.Pages;

public class AddTasksSelectTaskTypePage : ConsolePage
{
    private readonly Dictionary<string, GetQuotesTaskTypeEnum> _items;
    private readonly AddTasksRequestBillet _requestBillet;

    public AddTasksSelectTaskTypePage(
        ConsolePage? parent,
        AddTasksRequestBillet requestBillet) : base(parent)
    {
        _requestBillet = requestBillet;

        _items = new Dictionary<string, GetQuotesTaskTypeEnum>
        {
            { "Инициировать список инструментов провайдера", GetQuotesTaskTypeEnum.GetInitialData },
            { "Получать котировки в реальном времени", GetQuotesTaskTypeEnum.GetRealtimeQuotes },
            { "Загрузить историю котировок по инструменту", GetQuotesTaskTypeEnum.GetHistory }
        };
    }

    protected override void ShowInternal()
    {
        var itemName = AnsiConsole.Prompt(GetSelectionPrompt("Выбор типа задачи:", _items.Keys));
        _requestBillet.TaskType = _items[itemName];
        Parent!.Show();
    }
}