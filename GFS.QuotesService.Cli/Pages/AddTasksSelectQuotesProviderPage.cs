using GFS.ConsoleLibrary.Menu;
using GFS.QuotesService.Cli.Models;
using GFS.QuotesService.Common.Enum;
using Spectre.Console;

namespace GFS.QuotesService.Cli.Pages;

public class AddTasksSelectQuotesProviderPage : ConsolePage
{
    private readonly Dictionary<string, QuotesProviderTypeEnum> _items;
    private readonly AddTasksRequestBillet _requestBillet;

    public AddTasksSelectQuotesProviderPage(
        ConsolePage? parent,
        AddTasksRequestBillet requestBillet) : base(parent)
    {
        _requestBillet = requestBillet;

        _items = new Dictionary<string, QuotesProviderTypeEnum>();
        Enum.GetValues<QuotesProviderTypeEnum>().ToList().ForEach( e=> _items.Add(e.ToString(), e));
    }

    protected override void ShowInternal()
    {
        var itemName = AnsiConsole.Prompt(GetSelectionPrompt("Выбор поставщика котировок:", _items.Keys));
        _requestBillet.QuotesProviderType = _items[itemName];
        Parent!.Show();
    }
}