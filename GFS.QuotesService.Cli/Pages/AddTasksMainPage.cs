using GFS.ConsoleLibrary.Menu;
using GFS.QuotesService.Cli.Models;
using Spectre.Console;

namespace GFS.QuotesService.Cli.Pages;

public class AddTasksMainPage : ConsolePage
{
    private readonly Dictionary<string, Action> _items;

    public AddTasksMainPage(ConsolePage? parent) : base(parent)
    {
        var requestBillet = new AddTasksRequestBillet();

        _items = new Dictionary<string, Action>
        {
            { "Выбор типа задачи", new AddTasksSelectTaskTypePage(this, requestBillet).Show },
            { "Выбор поставщика котировок", new AddTasksSelectQuotesProviderPage(this, requestBillet).Show },
            { "Выбор инструментов", new AddTasksSelectAssetsPage(this, requestBillet).Show },
            { "Показать запрос", new AddTasksShowRequestPage(this, requestBillet).Show },
            { "Выполнить", new AddTasksExecutePage(this, requestBillet).Show },
            { "Назад", Parent!.Show }
        };
    }

    protected override void ShowInternal()
    {
        var itemName = AnsiConsole.Prompt(GetSelectionPrompt("Создание новой задачи для сервиса котировок", _items.Keys));
        _items[itemName]!.Invoke();
    }
}