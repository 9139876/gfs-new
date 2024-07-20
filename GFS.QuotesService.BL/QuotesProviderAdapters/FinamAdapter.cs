using GFS.Common.Exceptions;
using GFS.Common.Extensions;
using GFS.GrailCommon.Enums;
using GFS.GrailCommon.Models;
using GFS.QuotesService.Api.Enum;
using GFS.QuotesService.BL.Models.Adapters;
using GFS.QuotesService.BL.QuotesProviderAdapters.Abstraction;
using GFS.QuotesService.DAL.Entities;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Http;
using TinyCsvParser;
using TinyCsvParser.Mapping;

namespace GFS.QuotesService.BL.QuotesProviderAdapters;

internal interface IFinamAdapter : IQuotesProviderAdapter
{
}

internal class FinamAdapter : QuotesProviderAbstractAdapter, IFinamAdapter
{
    private readonly IHttpClientFactory _httpClientFactory;

    public FinamAdapter(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    protected override Task<GetQuotesBatchAdapterResponseModel> GetQuotesBatchInternal(GetQuotesDateBatchAdapterRequestModel request)
    {
        throw new NotImplementedYetException();
        
        // var data = await GetDataAsync(request);
        // var csvParser = new CsvParser<QuoteModel>(new CsvParserOptions(false, ';'), new CsvQuoteModelMapping());
        //
        // var result = csvParser
        //     .ReadFromString(new CsvReaderOptions(new[] { Environment.NewLine }), data)
        //     .Select(x => x.Result)
        //     .ToArray();
        //
        // throw new NotImplementedYetException();
    }

    public override ICollection<TimeFrameEnum> NativeSupportedTimeFrames => Enum.GetValues<FinamTimeFrameEnum>().Select(ConvertTimeFrame).ToArray();

    public override QuotesProviderTypeEnum ProviderType => QuotesProviderTypeEnum.Finam;

    private async Task<string> GetDataAsync(GetQuotesDateBatchAdapterRequestModel request)
    {
        using var httpClient = _httpClientFactory.CreateClient();

        var schemeAndHost = "http://export.finam.ru";
        var path = "export9.out";

        var uri = new Uri(new Uri(schemeAndHost), path);
        // uri.Query = GetQueryString(request.BatchBeginningDate, request.TimeFrame, request.Asset);

        // var path = (apiServiceType.GetCustomAttribute(typeof(RouteAttribute)) as RouteAttribute)?.Template ?? throw new InvalidOperationException();
        // path += $"/{ApiServiceParameters.PATH}";

        // return new Uri(new Uri(schemeAndHost), path);
        
        var uriBuilder = new UriBuilder()
        {
            Scheme = "https",
            Host = "export.finam.ru",
            Path = "export9.out",
            Query = GetQueryString(request.BatchBeginningDate, request.TimeFrame, request.Asset)
        };

        var httpRequest = new HttpRequestMessage();
        httpRequest.Method = HttpMethod.Get;
        httpRequest.RequestUri = uriBuilder.Uri;

        var channel = GrpcChannel.ForAddress(uri);
        await channel.ConnectAsync();
        var a = channel.Target;
        
        var str = uriBuilder.Uri.ToString();
        var httpResponse = await httpClient.GetAsync(str);
        
        // var httpResponse = await httpClient.SendAsync(httpRequest);

        if (!httpResponse.IsSuccessStatusCode)
            throw new InvalidOperationException();

        return await httpResponse.Content.ReadAsStringAsync();
    }

    private string GetQueryString(DateTime dateFrom, TimeFrameEnum timeFrame, AssetEntity asset)
    {
        if (asset.FinamEmitentIdentifier.IsEmptyOrWhiteSpace())
            throw new InvalidOperationException($"Идентификатор эмитента не указан у актива {asset.Name}");

        var dateTo = GetBatchEndDate(dateFrom, timeFrame);

        return QueryString.Create(new[]
        {
            new KeyValuePair<string, string>("p", $"{(int)ConvertTimeFrame(timeFrame)}"),
            new KeyValuePair<string, string>("datf", "5"),
            new KeyValuePair<string, string>("em", asset.FinamEmitentIdentifier!),
            new KeyValuePair<string, string>("yf", dateFrom.Year.ToString()),
            new KeyValuePair<string, string>("mf", (dateFrom.Month - 1).ToString()),
            new KeyValuePair<string, string>("df", dateFrom.Day.ToString()),
            new KeyValuePair<string, string>("yt", dateTo.Year.ToString()),
            new KeyValuePair<string, string>("mt", (dateTo.Month - 1).ToString()),
            new KeyValuePair<string, string>("dt", dateTo.Day.ToString())
        }).ToString();
    }

    private static TimeFrameEnum ConvertTimeFrame(FinamTimeFrameEnum finamTimeFrame)
    {
        return finamTimeFrame switch
        {
            FinamTimeFrameEnum.Tick1 => TimeFrameEnum.tick,
            FinamTimeFrameEnum.Min1 => TimeFrameEnum.min1,
            FinamTimeFrameEnum.Hour1 => TimeFrameEnum.H1,
            FinamTimeFrameEnum.Day1 => TimeFrameEnum.D1,
            FinamTimeFrameEnum.Week1 => TimeFrameEnum.W1,
            FinamTimeFrameEnum.Month1 => TimeFrameEnum.M1,
            _ => throw new ArgumentOutOfRangeException(nameof(finamTimeFrame), finamTimeFrame, null)
        };
    }

    private static FinamTimeFrameEnum ConvertTimeFrame(TimeFrameEnum timeFrame)
    {
        return timeFrame switch
        {
            TimeFrameEnum.tick => FinamTimeFrameEnum.Tick1,
            TimeFrameEnum.min1 => FinamTimeFrameEnum.Min1,
            TimeFrameEnum.H1 => FinamTimeFrameEnum.Hour1,
            TimeFrameEnum.D1 => FinamTimeFrameEnum.Day1,
            TimeFrameEnum.W1 => FinamTimeFrameEnum.Week1,
            TimeFrameEnum.M1 => FinamTimeFrameEnum.Month1,
            _ => throw new ArgumentOutOfRangeException(nameof(timeFrame), timeFrame, null)
        };
    }

    private static DateTime GetBatchEndDate(DateTime batchBeginningDate, TimeFrameEnum timeFrame)
    {
        return timeFrame switch
        {
            TimeFrameEnum.tick => batchBeginningDate.AddDays(1),

            TimeFrameEnum.min1
                or TimeFrameEnum.H1 => batchBeginningDate.AddMonths(3),

            TimeFrameEnum.D1
                or TimeFrameEnum.W1
                or TimeFrameEnum.M1 => batchBeginningDate.AddYears(4),

            _ => throw new ArgumentOutOfRangeException(nameof(timeFrame), timeFrame, null)
        };
    }
}

internal enum FinamTimeFrameEnum
{
    Tick1 = 1,
    Min1 = 2,
    Hour1 = 7,
    Day1 = 8,
    Week1 = 9,
    Month1 = 10
}

internal class CsvQuoteModelMapping : CsvMapping<QuoteModel>
{
    public CsvQuoteModelMapping()
    {
        MapProperty(0, q => q.Date);
        MapProperty(2, q => q.Open);
        MapProperty(3, q => q.High);
        MapProperty(4, q => q.Low);
        MapProperty(5, q => q.Close);
        MapProperty(6, q => q.Volume);
    }
}