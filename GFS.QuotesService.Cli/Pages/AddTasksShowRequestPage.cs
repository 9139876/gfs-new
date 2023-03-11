using GFS.Common.Extensions;
using GFS.ConsoleLibrary.Menu;
using GFS.QuotesService.Cli.Models;

namespace GFS.QuotesService.Cli.Pages;

public class AddTasksShowRequestPage: ConsolePage
{
    private readonly AddTasksRequestBillet _requestBillet;

    public AddTasksShowRequestPage(
        ConsolePage? parent,
        AddTasksRequestBillet requestBillet) : base(parent)
    {
        _requestBillet = requestBillet;
    }

    protected override void ShowInternal()
    {
        Console.Write(_requestBillet.CreateRequest().Serialize());
        
        AnyKeyAndReturnTo(Parent!);
    }
}