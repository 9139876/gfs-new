using GFS.Api.Client.Services;
using GFS.Common.Exceptions;
using GFS.GrailCommon.Extensions;
using GFS.GrailCommon.Models;
using GFS.QuotesService.Api.Interfaces;
using GFS.QuotesService.Api.Models;
using GFS.TradingStrategyTester.Common.Models;

namespace GFS.TradingStrategyTester.BL.Services;

public interface ITestTradeOrchestratorService
{
    Task Test(CommonSettings settings, Action<string?> updateState);
}

internal class TestTradeOrchestratorService : ITestTradeOrchestratorService
{
    private readonly IRemoteApiClient _remoteApiClient;

    public TestTradeOrchestratorService(IRemoteApiClient remoteApiClient)
    {
        _remoteApiClient = remoteApiClient;
    }

    public async Task Test(CommonSettings settings, Action<string?> updateState)
    {
        var assetsData = new Dictionary<Guid, AssetDataItem>();

        updateState("Получение котировок");
        
        foreach (var assetId in settings.TradingStrategyTesterSettings.AssetsIds)
        {
            var asset = (await _remoteApiClient.Call<GetAssetsInfo, AssetsFilter, List<AssetsInfoDto>>(new AssetsFilter { AssetId = assetId })).SingleOrDefault()
                        ?? throw new NotFoundException(typeof(AssetsInfoDto), assetId);

            var quoteRequest = new GetQuotesRequest
            {
                AssetId = assetId,
                TimeFrame = settings.TradingStrategyTesterSettings.TimeFrame,
                QuotesProviderType = settings.TradingStrategyTesterSettings.QuotesProviderType
            };

            var quotesDictionary = (await _remoteApiClient.Call<GetQuotes, GetQuotesRequest, GetQuotesResponse>(quoteRequest)).Quotes.ToDictionary(q => q.Date, q => q);

            assetsData.Add(assetId, new AssetDataItem { AssetsInfo = asset, QuotesDictionary = quotesDictionary });
        }

        var currentDate = assetsData.SelectMany(ad => ad.Value.QuotesDictionary.Keys).Min();
        var lastDate = assetsData.SelectMany(ad => ad.Value.QuotesDictionary.Keys).Max();

        while (currentDate <= lastDate) // && Portfolio говорит что еще не просрали все полимеры
        {
            updateState($"Текущая дата {currentDate.ToShortDateString()}, конечная дата {lastDate.ToShortDateString()}");
            
            // foreach (var assetId in settings.TradingStrategyTesterSettings.AssetsIds.Where(assetId => assetsData[assetId].QuotesDictionary.ContainsKey(currentDate)))
            // {
            //     var quote = assetsData[assetId].QuotesDictionary[currentDate];
            //     
            //     //Отложенные ордера
            //     //анализ
            //     //Анализ прогнозов
            //     //Совершение операций
            // }

            currentDate = currentDate.AddDate(settings.TradingStrategyTesterSettings.TimeFrame, 1);
        }

        updateState("Готово");
    }

    private class AssetDataItem
    {
#pragma warning disable CS8618

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public AssetsInfoDto AssetsInfo { get; init; }
        public Dictionary<DateTime, QuoteModel> QuotesDictionary { get; init; }

#pragma warning restore CS8618
    }
}