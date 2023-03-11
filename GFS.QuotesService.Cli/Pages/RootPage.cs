using GFS.ConsoleLibrary.Menu;
using Spectre.Console;

namespace GFS.QuotesService.Cli.Pages;

public class RootPage : ConsolePage
{
    private readonly Dictionary<string, Action> _items;

    public RootPage() : base(null)
    {
        _items = new Dictionary<string, Action>
        {
            { "Создать задачи", new AddTasksMainPage(this).Show },
            { "Отменить задачи", new CancelTasksPage(this).Show },
            { "Получить информацию по инструментам", new GetAssetsInfoPage(this).Show },
            { "Получить информацию по котировкам инструмента", new GetAssetQuotesInfoPage(this).Show },
            { "Получить список задач", new GetTasksPage(this).Show },
            { "Выход", () => { } }
        };
    }

    protected override void ShowInternal()
    {
        var itemName = AnsiConsole.Prompt(GetSelectionPrompt("Управление задачами сервиса котировок", _items.Keys));
        _items[itemName].Invoke();
    }
}