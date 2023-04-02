using System.IO.Compression;
using System.Net.Http.Headers;
using System.Web;
using AutoMapper;
using GFS.Common.Extensions;
using GFS.GrailCommon.Enums;
using GFS.GrailCommon.Models;
using GFS.QuotesService.Api.Common.Enum;
using GFS.QuotesService.Api.Enum;
using GFS.QuotesService.BL.Enum;
using GFS.QuotesService.BL.Models;
using GFS.QuotesService.BL.QuotesProviderAdapters.Abstraction;
using GFS.QuotesService.DAL.Entities;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Configuration;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;

namespace GFS.QuotesService.BL.QuotesProviderAdapters;

public interface ITinkoffAdapter : IQuotesProviderAdapter
{
}

internal class TinkoffAdapter : QuotesProviderAbstractAdapter, ITinkoffAdapter
{
    private readonly InvestApiClient _apiClient;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public TinkoffAdapter(
        InvestApiClient apiClient,
        IHttpClientFactory httpClientFactory,
        IMapper mapper,
        IConfiguration configuration)
    {
        _apiClient = apiClient;
        _httpClientFactory = httpClientFactory;
        _mapper = mapper;
        _configuration = configuration;
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

    protected override async Task<GetQuotesBatchResponseModel> GetQuotesBatchInternal(GetQuotesBatchRequestModel request)
    {
        return request.TimeFrame == TimeFrameEnum.min1
            ? await GetQuotesBatchFromArchive(request)
            : await GetQuotesBatchFromCandles(request);
    }

    public override TimeFrameEnum[] NativeSupportedTimeFrames => new[] { TimeFrameEnum.min1, TimeFrameEnum.H1, TimeFrameEnum.D1 };

    public override QuotesProviderTypeEnum ProviderType => QuotesProviderTypeEnum.Tinkoff;

    #region private

    private async Task<GetQuotesBatchResponseModel> GetQuotesBatchFromCandles(GetQuotesBatchRequestModel request)
    {
        //Работает в UTC
        var candlesRequest = new GetCandlesRequest
        {
            Figi = request.Asset.FIGI,
            Interval = TimeframeToCandleInterval(request.TimeFrame),
            From = request.TimeDirection == TimeDirectionEnum.Forward
                ? request.BatchBeginningDate.ToUniversalTime().ToTimestamp()
                : GetBatchStartDate(request.BatchBeginningDate, request.TimeFrame).ToUniversalTime().ToTimestamp(),
            To = request.TimeDirection == TimeDirectionEnum.Forward
                ? GetBatchEndDate(request.BatchBeginningDate, request.TimeFrame).ToUniversalTime().ToTimestamp()
                : request.BatchBeginningDate.ToUniversalTime().ToTimestamp()
        };

        var apiResponse = await _apiClient.MarketData.GetCandlesAsync(candlesRequest);
        apiResponse.RequiredNotNull();

        return new GetQuotesBatchResponseModel
        {
            Quotes = _mapper.Map<List<QuoteModel>>(apiResponse.Candles.ToList())
        };
    }

    private async Task<GetQuotesBatchResponseModel> GetQuotesBatchFromArchive(GetQuotesBatchRequestModel request)
    {
        if (request.TimeFrame != TimeFrameEnum.min1)
            throw new NotSupportedException($"{nameof(request.TimeFrame)} is {request.TimeFrame}");

        using var httpClient = _httpClientFactory.CreateClient();

        var token = _configuration.GetSection("TinkoffApiToken").Value ?? throw new InvalidOperationException("Tinkoff api key not specified in environment variables");
        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var tinkoffResponse = await httpClient.SendAsync(BuildRequestGetQuotesBatchFromArchive(request.Asset, request.BatchBeginningDate));

        var zipArchive = new ZipArchive(await tinkoffResponse.Content.ReadAsStreamAsync());

        var result = new List<QuoteModel>();

        foreach (var itemStream in zipArchive.Entries.Select(entry => entry.Open()))
        {
            using var sr = new StreamReader(itemStream);

            while (!sr.EndOfStream)
            {
                var itemDataLine = (await sr.ReadLineAsync())!.Split(';');

                result.Add(new QuoteModel
                {
                    TimeFrame = request.TimeFrame,
                    Date = DateTime.Parse(itemDataLine[1]).ToUniversalTime(),
                    Open = decimal.Parse(itemDataLine[2]),
                    Close = decimal.Parse(itemDataLine[3]),
                    High = decimal.Parse(itemDataLine[4]),
                    Low = decimal.Parse(itemDataLine[5]),
                    Volume = decimal.Parse(itemDataLine[6])
                });
            }
        }

        return new GetQuotesBatchResponseModel
        {
            Quotes = result
        };
    }

    private HttpRequestMessage BuildRequestGetQuotesBatchFromArchive(AssetEntity asset, DateTime batchEndDate)
    {
        var baseUri = _configuration.GetSection("TinkoffApi:UriHistoryData").Value ??
                      throw new InvalidOperationException("TinkoffApi.UriHistoryData not specified in configuration");

        var builder = new UriBuilder(baseUri);

        var query = HttpUtility.ParseQueryString(builder.Query);
        query["figi"] = asset.FIGI;
        query["year"] = batchEndDate.Year.ToString();

        builder.Query = query.ToString();

        var httpRequest = new HttpRequestMessage();
        httpRequest.Method = HttpMethod.Get;
        httpRequest.RequestUri = builder.Uri;

        return httpRequest;
    }

    #endregion

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

    private static DateTime GetBatchStartDate(DateTime end, TimeFrameEnum timeFrame)
        => timeFrame switch
        {
            TimeFrameEnum.min1 => end.AddDays(-1),
            TimeFrameEnum.H1 => end.AddDays(-7),
            TimeFrameEnum.D1 => end.AddDays(-365),
            _ => throw new ArgumentOutOfRangeException(nameof(timeFrame), timeFrame.ToString())
        };

    private static DateTime GetBatchEndDate(DateTime end, TimeFrameEnum timeFrame)
        => timeFrame switch
        {
            TimeFrameEnum.min1 => end.AddDays(1),
            TimeFrameEnum.H1 => end.AddDays(7),
            TimeFrameEnum.D1 => end.AddDays(365),
            _ => throw new ArgumentOutOfRangeException(nameof(timeFrame), timeFrame.ToString())
        };

    #endregion
}