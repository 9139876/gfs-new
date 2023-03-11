using GFS.ConsoleLibrary.Menu;
using GFS.QuotesService.Cli.Models;
using Spectre.Console;

namespace GFS.QuotesService.Cli.Pages;

public class AddTasksMainPage : ConsolePage
{
    private readonly Dictionary<string, Action> _items;
    private readonly AddTasksRequestBillet _requestBillet;

    public AddTasksMainPage(ConsolePage? parent) : base(parent)
    {
        _requestBillet = new AddTasksRequestBillet();
        
        _items = new Dictionary<string, Action>
        {
            { "Выбор типа задачи", new AddTasksSelectTaskTypePage(this, _requestBillet).Show },
            { "Выбор поставщика котировок", new AddTasksSelectQuotesProviderPage(this, _requestBillet).Show },
            { "Выбор инструментов", new AddTasksSelectAssetsPage(this, _requestBillet).Show },
            { "Показать запрос", new AddTasksShowRequestPage(this, _requestBillet).Show },
            { "Выполнить", new AddTasksExecutePage(this, _requestBillet).Show },
            { "Назад", Parent!.Show }
        };
    }

    protected override void ShowInternal()
    {
        var itemName = AnsiConsole.Prompt(GetSelectionPrompt("Создание новой задачи для сервиса котировок", _items.Keys));
        _items[itemName]!.Invoke();
    }
}