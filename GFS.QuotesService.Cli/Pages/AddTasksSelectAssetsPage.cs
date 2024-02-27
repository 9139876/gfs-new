using GFS.Api.Client.Services;
using GFS.ConsoleLibrary.Menu;
using GFS.QuotesService.Api.Interfaces;
using GFS.QuotesService.Api.Models;
using GFS.QuotesService.Cli.Models;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

namespace GFS.QuotesService.Cli.Pages;

public class AddTasksSelectAssetsPage: ConsolePage
{
    private readonly AddTasksRequestBillet _requestBillet;

    public AddTasksSelectAssetsPage(
        ConsolePage? parent,
        AddTasksRequestBillet requestBillet) : base(parent)
    {
        _requestBillet = requestBillet;
    }

    protected override void ShowInternal()
    {
        var items = new Dictionary<string, Guid>();

        if (!DoWithSpinner(() =>
            {
                var remoteApiClient = Program.ServiceProvider.GetRequiredService<IRemoteApiClient>();
                var apiResponse = remoteApiClient.Call<GetAssetsInfo, AssetsFilter, List<AssetsInfoDto>>(new AssetsFilter()).Result;
                items = apiResponse
                    .OrderBy(item => item.MarketType)
                    .ToDictionary(item => $"{item.MarketType} - {item.Ticker} - {item.Name}", item => item.AssetId);

                if (!items.Any())
                    throw new InvalidOperationException("Не найдено ни одного инструмента");

            }, "Получение данных о об инструментах...")) return;

        var itemNames = AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
                .Title("Выбор инструментов:")
                .NotRequired() 
                .PageSize(Math.Max(3, items.Count))
                .AddChoices(items.Keys));

        _requestBillet.AssetIds = itemNames.Select(name => items[name]).ToList();
        
        Parent!.Show();
    }
}