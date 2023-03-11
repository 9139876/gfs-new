using Spectre.Console;

namespace GFS.ConsoleLibrary.Menu;

public abstract class ConsolePage
{
    protected readonly ConsolePage? Parent;
    // private readonly List<MenuItem> _items = new();

    protected ConsolePage(ConsolePage? parent)
    {
        Parent = parent;
    }

    private bool IsRoot => Parent == null;
    protected ConsolePage Root => IsRoot ? this : Parent!.Root;

    protected void AnyKeyAndReturnTo(ConsolePage page)
    {
        Console.Write($"{Environment.NewLine}Press any key for return...");
        Console.ReadKey();

        page.Show();
    }

    protected static SelectionPrompt<string> GetSelectionPrompt(string title, params string[] items) =>
        new SelectionPrompt<string>()
            .HighlightStyle(new Style(AnsiConsole.Background, AnsiConsole.Foreground))
            .Title(title)
            .PageSize(Math.Max(3, items.Length))
            .AddChoices(items);

    protected static SelectionPrompt<string> GetSelectionPrompt(string title, IEnumerable<string> items) =>
        GetSelectionPrompt(title, items.ToArray());

    protected bool DoWithSpinner(Action action, string text = "Выполняется запрос...")
    {
        try
        {
            AnsiConsole.Status()
                .AutoRefresh(true)
                .Spinner(Spinner.Known.Aesthetic)
                .SpinnerStyle(Style.Parse("green bold"))
                .Start(text, _ => { action.Invoke(); });

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error - {e.Message}");
            AnyKeyAndReturnTo(Root);

            return false;
        }
    }

    public void Show()
    {
        AnsiConsole.Clear();
        ShowInternal();
    }

    protected abstract void ShowInternal();
}