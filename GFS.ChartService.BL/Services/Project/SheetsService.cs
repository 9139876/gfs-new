using System.Drawing;
using System.Globalization;
using GFS.Api.Client.Services;
using GFS.ChartService.BL.Models.ProjectViewModel.Sheet;
using GFS.ChartService.BL.Models.ProjectViewModel.Sheet.Layers;
using GFS.ChartService.BL.Models.Requests;
using GFS.GrailCommon.Enums;
using GFS.GrailCommon.Extensions;
using GFS.GrailCommon.Models;
using GFS.QuotesService.Api.Interfaces;
using GFS.QuotesService.Api.Models;

namespace GFS.ChartService.BL.Services.Project;

public interface ISheetsService
{
    Task<SheetViewModel> CreateSheet(CreateSheetRequest request, Guid clientId);
}

internal class SheetsService : ISheetsService
{
    private const int MIN_QUOTES_COUNT = 50;
    private const int DEFAULT_OFFSET = 10;

    private readonly ISessionService _sessionService;
    private readonly IRemoteApiClient _remoteApiClient;

    public SheetsService(
        ISessionService sessionService,
        IRemoteApiClient remoteApiClient)
    {
        _sessionService = sessionService;
        _remoteApiClient = remoteApiClient;
    }

    public async Task<SheetViewModel> CreateSheet(CreateSheetRequest request, Guid clientId)
    {
        var quotesRequest = new GetQuotesRequest
        {
            AssetId = request.AssetId,
            TimeFrame = request.TimeFrame,
            StartDate = request.StartDate
        };

        var quotesResponse = await _remoteApiClient.Call<GetQuotes, GetQuotesRequest, GetQuotesResponse>(quotesRequest);

        if (quotesResponse.Quotes.Count < MIN_QUOTES_COUNT)
            throw new InvalidOperationException($"Установлено минимальное количество котировок {MIN_QUOTES_COUNT}, а получено {quotesResponse.Quotes.Count}");

        var sheetSize = GetSheetSize(quotesResponse.Quotes, request.KPrice, request.RightEmptySpace);

        var result = new SheetViewModel
        {
            Name = request.Name,
            Size = sheetSize,
            TrackerData = GetTrackerData(sheetSize, request.KPrice, quotesResponse.Quotes, request.TimeFrame)
        };

        return result;
    }

    private static Size GetSheetSize(IReadOnlyCollection<QuoteModel> quoteModels, decimal kPrice, int rightEmptySpace)
    {
        // ReSharper disable once PossibleLossOfFraction
        var width = Math.Ceiling(quoteModels.Count / (decimal)10) * 10 + rightEmptySpace + DEFAULT_OFFSET;
        var height = Math.Ceiling(quoteModels.Max(quote => quote.High) * kPrice / 10) * 10 + DEFAULT_OFFSET;

        return new Size((int)width, (int)height);
    }

    private static TrackerDataViewModel GetTrackerData(Size sheetSize, decimal kPrice, IReadOnlyCollection<QuoteModel> quotes, TimeFrameEnum timeFrame)
    {
        var trackerData = new TrackerDataViewModel
        {
            PriceValues = GetTrackerPriceValues(sheetSize.Height, kPrice).ToArray(),
            TimeValues = GetTrackerTimeValues(sheetSize.Width, quotes, timeFrame).ToArray()
        };

        return trackerData;
    }

    private static IEnumerable<DateTime> GetTrackerTimeValues(int width, IReadOnlyCollection<QuoteModel> quotes, TimeFrameEnum timeFrame)
    {
        var currentQuoteDate = quotes.First().Date;

        using var quotesEnumerator = quotes.GetEnumerator();

        for (var i = 0; i < width; i++)
        {
            if (i < DEFAULT_OFFSET)
                yield return currentQuoteDate.AddDate(timeFrame, i - DEFAULT_OFFSET);
            else
            {
                currentQuoteDate = quotesEnumerator.MoveNext() ? quotesEnumerator.Current.Date : currentQuoteDate.AddDate(timeFrame, 1);
                yield return currentQuoteDate;
            }
        }
    }

    private static IEnumerable<string> GetTrackerPriceValues(int height, decimal kPrice)
    {
        decimal current = 0;

        for (var i = 0; i < height; i++)
        {
            var roundedCurrent = Math.Round(current * (decimal)Math.Pow(10, 5)) / (decimal)Math.Pow(10, 5);
            yield return roundedCurrent.ToString(CultureInfo.InvariantCulture);

            current += 1 / kPrice;
        }
    }
}