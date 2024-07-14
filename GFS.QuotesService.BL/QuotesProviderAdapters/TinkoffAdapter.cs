using System.IO.Compression;
using System.Net;
using System.Net.Http.Headers;
using System.Web;
using AutoMapper;
using GFS.Common.Extensions;
using GFS.GrailCommon.Enums;
using GFS.GrailCommon.Extensions;
using GFS.GrailCommon.Models;
using GFS.QuotesService.Api.Enum;
using GFS.QuotesService.BL.Models;
using GFS.QuotesService.BL.Models.Adapters;
using GFS.QuotesService.BL.QuotesProviderAdapters.Abstraction;
using GFS.QuotesService.DAL.Entities;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Configuration;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;

#pragma warning disable CS0612

namespace GFS.QuotesService.BL.QuotesProviderAdapters;

internal interface ITinkoffAdapter : IQuotesProviderAdapter
{
}

internal class TinkoffAdapter : QuotesProviderAbstractAdapter, ITinkoffAdapter
{
    private readonly InvestApiClient _apiClient;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMapper _mapper;
    private readonly string _historyDataBaseUri;
    private readonly string _token;

    public TinkoffAdapter(
        InvestApiClient apiClient,
        IHttpClientFactory httpClientFactory,
        IMapper mapper,
        IConfiguration configuration)
    {
        _apiClient = apiClient;
        _httpClientFactory = httpClientFactory;
        _mapper = mapper;

        _historyDataBaseUri = configuration.GetSection("TinkoffApi:UriHistoryData").Value
                              ?? throw new InvalidOperationException("TinkoffApi.UriHistoryData not specified in configuration");

        _token = configuration.GetSection("TinkoffApiToken").Value
                 ?? throw new InvalidOperationException("Tinkoff api key not specified in environment variables");
    }

    public override async Task<List<AssetModel>> GetAssetsData()
    {
        var result = new List<AssetModel>();

        var sharesResponse = await _apiClient.Instruments.SharesAsync();
        sharesResponse.RequiredNotNull();
        var shares = _mapper.Map<List<AssetModel>>(sharesResponse.Instruments.Where(s => s != null).ToList());
        shares.ForEach(share =>
        {
            share.AssetType = AssetTypeEnum.Shares;
            share.MarketType = GetMarketType(share.Exchange);
        });
        result.AddRange(shares);

        var currenciesResponse = await _apiClient.Instruments.CurrenciesAsync(new InstrumentsRequest());
        currenciesResponse.RequiredNotNull();
        var currencies = _mapper.Map<List<AssetModel>>(currenciesResponse.Instruments.Where(s => s != null).ToList());
        currencies.ForEach(currency =>
        {
            currency.AssetType = AssetTypeEnum.Currencies;
            currency.MarketType = GetMarketType(currency.Exchange);
        });
        result.AddRange(currencies);

        // Вроде не нужно
        // var etfsResponse = await _apiClient.Instruments.EtfsAsync();
        // etfsResponse.RequiredNotNull();
        // var etfs = _mapper.Map<List<AssetModel>>(etfsResponse.Instruments.Where(s => s != null).ToList());
        // etfs.ForEach(etf =>
        // {
        //     etf.AssetType = AssetTypeEnum.Etfs;
        //     etf.MarketType = GetMarketType(etf.Exchange);
        // });
        // result.AddRange(etfs);

        return result;
    }

    public override async Task<DateTime> GetFirstQuoteDate(GetFirstQuoteDateAdapterRequestModel dateAdapterRequest)
    {
        var firstCandleDateApiResponse =
            await _apiClient.Instruments.GetInstrumentByAsync(new InstrumentRequest { IdType = InstrumentIdType.Figi, Id = dateAdapterRequest.Asset.FIGI });
        firstCandleDateApiResponse.RequiredNotNull();

        var firstQuoteDate = dateAdapterRequest.TimeFrame switch
        {
            TimeFrameEnum.min1 or TimeFrameEnum.H1 => firstCandleDateApiResponse.Instrument.First1MinCandleDate.ToDateTime(),
            TimeFrameEnum.D1 => firstCandleDateApiResponse.Instrument.First1DayCandleDate.ToDateTime(),
            _ => throw new ArgumentOutOfRangeException()
        };

        return firstQuoteDate;
    }

    protected override async Task<GetQuotesBatchAdapterResponseModel> GetQuotesBatchInternal(GetQuotesDateBatchAdapterRequestModel request)
    {
        if (request.TimeFrame == TimeFrameEnum.min1)
            return await GetQuotesBatchFromArchive(request);

        var attempts = request.TimeFrame switch
        {
            TimeFrameEnum.H1 => 3,
            TimeFrameEnum.D1 => 1,
            _ => throw new ArgumentOutOfRangeException(nameof(request.TimeFrame), request.TimeFrame.ToString())
        };

        while (attempts > 0)
        {
            var response = await GetQuotesBatchFromCandles(request);
            if (response.Quotes.Any())
                return response;

            request.BatchBeginningDate = response.NextBatchBeginningDate!.Value;
            attempts--;
        }

        return new GetQuotesBatchAdapterResponseModel
        {
            Quotes = new List<QuoteModel>(),
            NextBatchBeginningDate = null
        };
    }

    // public override ICollection<TimeFrameEnum> NativeSupportedTimeFrames => new[] { TimeFrameEnum.min1, TimeFrameEnum.H1, TimeFrameEnum.D1 };
    public override ICollection<TimeFrameEnum> NativeSupportedTimeFrames => new[] { TimeFrameEnum.D1 };

    public override QuotesProviderTypeEnum ProviderType => QuotesProviderTypeEnum.Tinkoff;

    #region private

    private async Task<GetQuotesBatchAdapterResponseModel> GetQuotesBatchFromCandles(GetQuotesDateBatchAdapterRequestModel request)
    {
        //Работает в UTC
        var candlesRequest = new GetCandlesRequest
        {
            Figi = request.Asset.FIGI,
            Interval = TimeframeToCandleInterval(request.TimeFrame),
            From = request.BatchBeginningDate.ToUniversalTime().ToTimestamp(),
            To = GetBatchEndDate(request.BatchBeginningDate, request.TimeFrame).ToUniversalTime().ToTimestamp()
        };

        var apiResponse = await _apiClient.MarketData.GetCandlesAsync(candlesRequest);
        apiResponse.RequiredNotNull();

        return new GetQuotesBatchAdapterResponseModel
        {
            Quotes = _mapper.Map<List<QuoteModel>>(apiResponse.Candles.ToList()),
            NextBatchBeginningDate = candlesRequest.To.ToDateTime().AddDate(request.TimeFrame, 1)
        };
    }

    private async Task<GetQuotesBatchAdapterResponseModel> GetQuotesBatchFromArchive(GetQuotesDateBatchAdapterRequestModel request)
    {
        if (request.TimeFrame != TimeFrameEnum.min1)
            throw new NotSupportedException($"{nameof(request.TimeFrame)} is {request.TimeFrame}");

        using var httpClient = _httpClientFactory.CreateClient();

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

        var tinkoffResponse = await httpClient.SendAsync(BuildRequestGetQuotesBatchFromArchive(request.Asset, request.BatchBeginningDate));

        if (!tinkoffResponse.IsSuccessStatusCode)
        {
            //Достал, пришлось захардкодить - это ошибка что год неправильный (просто за этот год котировок нет) 
            if (tinkoffResponse.StatusCode == HttpStatusCode.NotFound ||
                (tinkoffResponse.Headers.TryGetValues("code", out var tinkoffCodes) && tinkoffCodes.FirstOrDefault() == "30086"))
                return new GetQuotesBatchAdapterResponseModel
                {
                    Quotes = new List<QuoteModel>(),
                    NextBatchBeginningDate = null
                };

            var error = "Unknown";

            if (tinkoffResponse.Headers.TryGetValues("message", out var errors))
                error = errors.FirstOrDefault() ?? error;

            throw new InvalidOperationException($"Tinkoff api response code is {tinkoffResponse.StatusCode}, error: {error}");
        }

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

        return new GetQuotesBatchAdapterResponseModel
        {
            Quotes = result,
            NextBatchBeginningDate = request.BatchBeginningDate.AddYears(1)
        };
    }

    private HttpRequestMessage BuildRequestGetQuotesBatchFromArchive(AssetEntity asset, DateTime batchEndDate)
    {
        var builder = new UriBuilder(_historyDataBaseUri);

        var query = HttpUtility.ParseQueryString(builder.Query);
        query["figi"] = asset.FIGI;
        query["year"] = batchEndDate.Year.ToString();

        builder.Query = query.ToString();

        var httpRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = builder.Uri
        };

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