using AutoMapper;
using GFS.Common.Extensions;
using GFS.GrailCommon.Enums;
using GFS.GrailCommon.Extensions;
using GFS.GrailCommon.Models;
using GFS.QuotesService.Api.Enum;
using GFS.QuotesService.BL.Models;
using GFS.QuotesService.BL.QuotesProviderAdapters.Abstraction;
using GFS.QuotesService.DAL.Entities;
using Google.Protobuf.WellKnownTypes;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;

namespace GFS.QuotesService.BL.QuotesProviderAdapters;

public interface ITinkoffAdapter : IQuotesProviderAdapter
{
}

public class TinkoffAdapter : QuotesProviderAbstractAdapter, ITinkoffAdapter
{
    private readonly InvestApiClient _apiClient;
    private readonly IMapper _mapper;

    public TinkoffAdapter(
        InvestApiClient apiClient,
        IMapper mapper)
    {
        _apiClient = apiClient;
        _mapper = mapper;
    }

    public override async Task<List<InitialModel>> GetInitialData()
    {
        var result = new List<InitialModel>();

        var sharesResponse = await _apiClient.Instruments.SharesAsync();
        sharesResponse.RequiredNotNull();
        var shares = _mapper.Map<List<InitialModel>>(sharesResponse.Instruments.Where(s => s != null).ToList());
        shares.ForEach(share =>
        {
            share.AssetType = AssetTypeEnum.Shares;
            share.MarketType = GetMarketType(share.Exchange);
        });
        result.AddRange(shares);

        var currenciesResponse = await _apiClient.Instruments.CurrenciesAsync(new InstrumentsRequest());
        currenciesResponse.RequiredNotNull();
        var currencies = _mapper.Map<List<InitialModel>>(currenciesResponse.Instruments.Where(s => s != null).ToList());
        currencies.ForEach(currency =>
        {
            currency.AssetType = AssetTypeEnum.Currencies;
            currency.MarketType = GetMarketType(currency.Exchange);
        });
        result.AddRange(currencies);

        var etfsResponse = await _apiClient.Instruments.EtfsAsync();
        etfsResponse.RequiredNotNull();
        var etfs = _mapper.Map<List<InitialModel>>(etfsResponse.Instruments.Where(s => s != null).ToList());
        etfs.ForEach(etf =>
        {
            etf.AssetType = AssetTypeEnum.Etfs;
            etf.MarketType = GetMarketType(etf.Exchange);
        });
        result.AddRange(etfs);

        return result;
    }

    protected override async Task<IEnumerable<QuoteModel>> GetQuotesBatchInternal(AssetEntity asset, TimeFrameEnum timeFrame, DateTime lastQuoteDate)
    {
        //Работает в UTC
        var request = new GetCandlesRequest()
        {
            Figi = asset.FIGI,
            Interval = TimeframeToCandleInterval(timeFrame),
            From = lastQuoteDate.AddDate(timeFrame,1).ToUniversalTime().ToTimestamp(),
            To = GetEndPeriodDate(lastQuoteDate, timeFrame).ToUniversalTime().ToTimestamp()
        };

        var apiResponse = await _apiClient.MarketData.GetCandlesAsync(request);
        apiResponse.RequiredNotNull();

        return _mapper.Map<List<QuoteModel>>(apiResponse.Candles.ToList());
    }

    protected override TimeFrameEnum[] NativeSupportedTimeFrames => new[] { TimeFrameEnum.min1, TimeFrameEnum.H1, TimeFrameEnum.D1 };

    #region static

    private static MarketTypeEnum GetMarketType(string exchange)
        => exchange switch
        {
            _ when exchange.ToUpper().StartsWith("MOEX") => MarketTypeEnum.MOEX,
            _ when exchange.ToUpper().StartsWith("SPB") => MarketTypeEnum.SPB,
            _ when exchange.ToUpper().StartsWith("LSE") => MarketTypeEnum.LSE,
            _ => MarketTypeEnum.Unknown
        };

    private static CandleInterval TimeframeToCandleInterval(TimeFrameEnum timeFrame)
        => timeFrame switch
        {
            TimeFrameEnum.min1 => CandleInterval._1Min,
            TimeFrameEnum.H1 => CandleInterval.Hour,
            TimeFrameEnum.D1 => CandleInterval.Day,
            _ => throw new ArgumentOutOfRangeException(nameof(timeFrame), timeFrame.ToString())
        };

    private static DateTime GetEndPeriodDate(DateTime start, TimeFrameEnum timeFrame)
        => timeFrame switch
        {
            TimeFrameEnum.min1 => start.AddDays(1),
            TimeFrameEnum.H1 => start.AddDays(7),
            TimeFrameEnum.D1 => start.AddDays(365),
            _ => throw new ArgumentOutOfRangeException(nameof(timeFrame), timeFrame.ToString())
        };

    #endregion
}