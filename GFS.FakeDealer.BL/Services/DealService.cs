using GFS.Api.Client.Services;
using GFS.FakeDealer.Api.Enums;
using GFS.FakeDealer.Api.Models;
using GFS.QuotesService.Api.Interfaces;
using GFS.QuotesService.Api.Models;

namespace GFS.FakeDealer.BL.Services;

public interface IDealService
{
    Task<MakeDealResponse> MakeDeal(MakeDealRequest request);
}

internal class DealService : IDealService
{
    private readonly ISettingsService _settingsService;
    private readonly IRemoteApiClient _remoteApiClient;

    public DealService(
        ISettingsService settingsService,
        IRemoteApiClient remoteApiClient)
    {
        _settingsService = settingsService;
        _remoteApiClient = remoteApiClient;
    }

    public async Task<MakeDealResponse> MakeDeal(MakeDealRequest request)
    {
        var settings = _settingsService.GetDealerSettings();
        var assetId = (await _remoteApiClient.Call<GetAssetsInfo, AssetsFilter, List<AssetsInfoDto>>(new AssetsFilter { FIGI = request.FIGI })).Single().AssetId;

        var quoteRequest = new GetQuotesRequest
        {
            AssetId = assetId,
            TimeFrame = settings.TimeFrame,
            QuotesProviderType = settings.QuotesProviderType,
            StartDate = request.DealDateUtc,
            EndDate = request.DealDateUtc
        };

        var quote = (await _remoteApiClient.Call<GetQuotes, GetQuotesRequest, GetQuotesResponse>(quoteRequest)).Quotes.Single();
        var dealPrice = _settingsService.GetDealPriceCalculator(request.OperationType)(quote);
        var cashAmount = request.AssetUnitsCount * dealPrice;

        var multiplier = request.OperationType == DealerOperationTypeEnum.Sell ? 1 : -1;

        return new MakeDealResponse
        {
            DealPrice = dealPrice,
            DeltaCash = cashAmount * multiplier,
            DeltaAsset = request.AssetUnitsCount * multiplier * -1
        };
    }
}