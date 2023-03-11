using GFS.Api.Client.Services;
using GFS.Common.Extensions;
using GFS.ConsoleLibrary.Menu;
using GFS.QuotesService.Api.Interfaces;
using GFS.QuotesService.Api.Models;
using Microsoft.Extensions.DependencyInjection;

namespace GFS.QuotesService.Cli.Pages;

public class GetAssetsInfoPage : ConsolePage
{
    public GetAssetsInfoPage(ConsolePage? parent) : base(parent)
    {
    }

    protected override void ShowInternal()
    {
        var response = string.Empty;

        if (!DoWithSpinner(() =>
            {
                var remoteApiClient = Program.ServiceProvider.GetRequiredService<IRemoteApiClient>();
                var apiResponse = remoteApiClient.Call<GetAssetsInfo, AssetsFilter, List<AssetsInfoDto>>(new AssetsFilter()).Result;
                response = apiResponse.Serialize();
            })) return;

        Console.WriteLine(response);
        AnyKeyAndReturnTo(Root);
    }
}