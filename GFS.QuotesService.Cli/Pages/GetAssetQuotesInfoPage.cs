using GFS.Api.Client.Services;
using GFS.Common.Extensions;
using GFS.ConsoleLibrary.Menu;
using GFS.QuotesService.Api.Interfaces;
using GFS.QuotesService.Api.Models;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

namespace GFS.QuotesService.Cli.Pages;

public class GetAssetQuotesInfoPage : ConsolePage
{
    public GetAssetQuotesInfoPage(ConsolePage? parent) : base(parent)
    {
    }

    protected override void ShowInternal()
    {
        var items = new Dictionary<string, Guid>();

        if (!DoWithSpinner(() =>
            {
                var remoteApiClient = Program.ServiceProvider.GetRequiredService<IRemoteApiClient>();
                var apiResponse = remoteApiClient.Call<GetAssetsInfo, AssetsFilter, List<AssetsInfoDto>>(new AssetsFilter { OnlyHasQuotes = true }).Result;
                items = apiResponse
                    .OrderBy(item => item.MarketType)
                    .ToDictionary(item => $"{item.MarketType} - {item.Ticker} - {item.Name}", item => item.AssetId);
            }, "Получение данных о об инструментах...")) return;

        var itemName = AnsiConsole.Prompt(GetSelectionPrompt("Получение информации о котировках инструмента. Выбор инструмента:", items.Keys));

        var response = string.Empty;

        if (!DoWithSpinner(() =>
            {
                var remoteApiClient = Program.ServiceProvider.GetRequiredService<IRemoteApiClient>();
                var apiResponse = remoteApiClient.Call<GetAssetQuotesInfo, Guid, AssetQuotesInfoDto>(items[itemName]).Result;
                response = apiResponse.Serialize();
            })) return;


        Console.WriteLine(response);
        AnyKeyAndReturnTo(Root);
    }
}