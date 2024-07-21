using GFS.Broker.Api.Models;
using GFS.Broker.Api.Models.TestDealer;
using GFS.Common.Exceptions;

namespace GFS.Broker.BL.Services;

public interface IDealService
{
    Task<MakeDealResponse> MakeDeal(MakeDealRequest request);
    Task<TryPerformPendingOrdersResponse> TryPerformPendingOrders(TryPerformPendingOrdersRequest request);
}

internal class TestDealService : IDealService
{
    // private readonly IPortfolioService _portfolioService;
    // private readonly IMapper _mapper;
    // private readonly ISettingsService _settingsService;
    // private readonly IRemoteApiClient _remoteApiClient;
    //
    // public TestDealService(
    //     IPortfolioService portfolioService,
    //     IMapper mapper,
    //     ISettingsService settingsService,
    //     IRemoteApiClient remoteApiClient)
    // {
    //     _portfolioService = portfolioService;
    //     _mapper = mapper;
    //     _settingsService = settingsService;
    //     _remoteApiClient = remoteApiClient;
    // }

    public Task<MakeDealResponse> MakeDeal(MakeDealRequest request)
    {
        throw new NotImplementedException();

        // var settings = _settingsService.GetDealerSettings();
        // var assetId = (await _remoteApiClient.Call<GetAssetsInfo, AssetsFilter, List<AssetsInfoDto>>(new AssetsFilter { FIGI = request.FIGI })).Single().AssetId;
        //
        // !!!
        //     var quoteRequest = new GetQuotesRequest
        // {
        //     AssetId = assetId,
        //     TimeFrame = settings.TimeFrame,
        //     QuotesProviderType = settings.QuotesProviderType,
        //     StartDate = request.DealDateUtc,
        //     EndDate = request.DealDateUtc
        // };
        //
        // var quote = (await _remoteApiClient.Call<GetQuotes, GetQuotesRequest, GetQuotesResponse>(quoteRequest)).Quotes.Single();
        //
        // var dealPrice = _settingsService.GetDealPriceCalculator(request.OperationType)(quote);
        // var cashAmount = request.AssetUnitsCount * dealPrice;
        // var dealerCommission = _settingsService.GetDealerCommission(cashAmount);
        //
        // var cashMultiplier = request.OperationType == DealOperationType.Sell ? 1 : -1;
        // var assetMultiplier = cashMultiplier * -1;
        //
        // return new MakeDealResponse
        // {
        //     DealPrice = dealPrice,
        //     DeltaCash = cashAmount * cashMultiplier - dealerCommission,
        //     DeltaAsset = request.AssetUnitsCount * assetMultiplier
        // };
    }

    public Task<TryPerformPendingOrdersResponse> TryPerformPendingOrders(TryPerformPendingOrdersRequest request)
    {
        throw new NotImplementedException();

        // using var transaction = SystemTransaction.Default();
        //
        // var portfolioInfo = await _portfolioService.GetPortfolioInfo(_mapper.Map<GetPortfolioInfoRequestDto>(request));
        //
        // var assetIds = request.AssetsWithQuotes.Select(awq => awq.AssetId);
        //
        // var ordersToRemove = new List<PendingOrderDto>();
        //
        // foreach (var order in portfolioInfo.PendingOrders.Where(po => assetIds.Contains(po.AssetId)))
        // {
        //     var portfolioAssetPosition = portfolioInfo.PortfolioState.Assets.SingleOrDefault(asset => asset.AssetId == order.AssetId);
        //
        //     if (portfolioAssetPosition == null)
        //     {
        //          ordersToRemove.Add(order);
        //         continue;
        //     }
        //     
        //     var quote = request.AssetsWithQuotes.Single(awq => awq.AssetId == order.AssetId).Quote;
        //     if (order.Price >= quote.Low && order.Price <= quote.High)
        //     {
        //         _portfolioService.PerformDealOperation()    
        //     }
        // }
        //
        // transaction.Complete();
    }
}

internal class RealDealService : IDealService
{
    public Task<MakeDealResponse> MakeDeal(MakeDealRequest request)
    {
        throw new NotImplementedYetException();
    }

    public Task<TryPerformPendingOrdersResponse> TryPerformPendingOrders(TryPerformPendingOrdersRequest request)
    {
        throw new InvalidOperationException("Неприменимо для реального брокера");
    }
}