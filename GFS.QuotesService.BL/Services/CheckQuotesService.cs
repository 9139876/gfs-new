using AutoMapper;
using GFS.Common.Exceptions;
using GFS.Common.Extensions;
using GFS.Common.Helpers;
using GFS.EF.Repository;
using GFS.GrailCommon.Enums;
using GFS.GrailCommon.Extensions;
using GFS.GrailCommon.Models;
using GFS.QuotesService.Api.Models.CheckQuotes;
using GFS.QuotesService.BL.Helpers;
using GFS.QuotesService.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace GFS.QuotesService.BL.Services;

public interface ICheckQuotesService
{
    Task<CompareTimeframesCheckQuotesResponse> CompareTimeframesCheckQuotes(CompareTimeframesCheckQuotesRequest request);
}

internal class CheckQuotesService : ICheckQuotesService
{
    private const int MAX_DIFFERENCES = 128;

    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public CheckQuotesService(
        IDbContext dbContext,
        IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<CompareTimeframesCheckQuotesResponse> CompareTimeframesCheckQuotes(CompareTimeframesCheckQuotesRequest request)
    {
        if ((request.LastDate.Date - request.FirstDate.Date).TotalDays < 1)
            throw new InvalidOperationException($"{nameof(request.LastDate)} должна быть больше {nameof(request.FirstDate)} хотя бы на 1 день");

        var assetProviderCompatibilityEntities = await _dbContext.GetRepository<AssetProviderCompatibilityEntity>()
            .Get(e => new[] { request.AssetWithProviderOneId, request.AssetWithProviderTwoId }.Contains(e.Id))
            .Include(e => e.Asset)
            .AsNoTracking()
            .ToListAsync();

        var assetWithProviderOne = assetProviderCompatibilityEntities.SingleOrDefault(x => x.Id == request.AssetWithProviderOneId)
                                   ?? throw new NotFoundException(typeof(AssetProviderCompatibilityEntity), request.AssetWithProviderOneId);

        var assetWithProviderTwo = assetProviderCompatibilityEntities.SingleOrDefault(x => x.Id == request.AssetWithProviderTwoId)
                                   ?? throw new NotFoundException(typeof(AssetProviderCompatibilityEntity), request.AssetWithProviderTwoId);

        var result = new CompareTimeframesCheckQuotesResponse
        (
            one: new AssetProviderTimeFrame(assetWithProviderOne.Asset!.Name, assetWithProviderOne.QuotesProviderType, assetWithProviderOne.TimeFrame),
            two: new AssetProviderTimeFrame(assetWithProviderTwo.Asset!.Name, assetWithProviderTwo.QuotesProviderType, assetWithProviderTwo.TimeFrame),
            targetTimeFrame: (TimeFrameEnum)Math.Max((int)assetWithProviderOne.TimeFrame, (int)assetWithProviderTwo.TimeFrame)
        );

        var baseQuotesQuery = _dbContext.GetRepository<QuoteEntity>()
            .Get(q => q.Date >= request.FirstDate.StartOfDay() && q.Date <= request.LastDate.EndOfDay())
            .AsNoTracking();

        var quotesOne = await baseQuotesQuery
            .Where(q => q.AssetId == assetWithProviderOne.AssetId
                        && q.TimeFrame == assetWithProviderOne.TimeFrame
                        && q.QuotesProviderType == assetWithProviderOne.QuotesProviderType)
            .ToListAsync();

        if (!quotesOne.Any())
        {
            result.Summary = "Для первой комбинации актива, провайдера и таймфрейма не найдено ни одной котировки, попадающей в заданный временной диапазон";
            return result;
        }

        var quotesTwo = await baseQuotesQuery
            .Where(q => q.AssetId == assetWithProviderTwo.AssetId
                        && q.TimeFrame == assetWithProviderTwo.TimeFrame
                        && q.QuotesProviderType == assetWithProviderTwo.QuotesProviderType)
            .ToListAsync();

        if (!quotesTwo.Any())
        {
            result.Summary = "Для второй комбинации актива, провайдера и таймфрейма не найдено ни одной котировки, попадающей в заданный временной диапазон";
            return result;
        }

        var quotesModelsOne = (assetWithProviderOne.TimeFrame < result.TargetTimeFrame
                ? QuotesTimeframeConverter.ConvertToTimeframe(_mapper.Map<List<QuoteModel>>(quotesOne), assetWithProviderOne.TimeFrame, result.TargetTimeFrame)
                : _mapper.Map<List<QuoteModel>>(quotesOne))
            .ToDictionary(x => x.Date, x => x);

        var quotesModelsTwo = (assetWithProviderTwo.TimeFrame < result.TargetTimeFrame
                ? QuotesTimeframeConverter.ConvertToTimeframe(_mapper.Map<List<QuoteModel>>(quotesTwo), assetWithProviderTwo.TimeFrame, result.TargetTimeFrame)
                : _mapper.Map<List<QuoteModel>>(quotesTwo))
            .ToDictionary(x => x.Date, x => x);

        result.FirstDate = DateTimeHelpers.MaxDate(quotesModelsOne.Select(x => x.Key).OrderBy(x => x).First(), quotesModelsTwo.Select(x => x.Key).OrderBy(x => x).First());
        result.LastDate = DateTimeHelpers.MinDate(quotesModelsOne.Select(x => x.Key).OrderBy(x => x).Last(), quotesModelsTwo.Select(x => x.Key).OrderBy(x => x).Last());

        if ((result.LastDate.Date - result.FirstDate.Date).TotalDays < 1)
        {
            result.Summary = "Слишком мало котировок для сравнения";
            return result;
        }

        var date = result.FirstDate;

        while (date <= result.LastDate)
        {
            if (quotesModelsOne.TryGetValue(date, out var modelOne) | quotesModelsTwo.TryGetValue(date, out var modelTwo))
            {
                var differentItems = CompareQuotes(modelOne, modelTwo, request.AllowableDifferenceInPercents);

                if (differentItems.Any())
                {
                    result.Differences.Add(new CompareQuotesDifferent(date, differentItems));

                    if (result.Differences.Count >= MAX_DIFFERENCES)
                    {
                        result.Summary = $"Превышено максимальное количество расхождений - {MAX_DIFFERENCES}";
                        return result;
                    }
                }
            }

            date = date.AddDate(result.TargetTimeFrame, 1);
        }

        result.Summary = result.Differences.Any()
            ? $"Имеется {result.Differences.Count} расхождений"
            : "Расхождений нет";

        return result;
    }

    private List<CompareQuotesDifferentItem> CompareQuotes(QuoteModel? quoteOne, QuoteModel? quoteTwo, decimal allowableDifference)
    {
        if (quoteOne is null)
            return new List<CompareQuotesDifferentItem> { new CompareQuotesDifferentItem("quote", "null", "notNull") };

        if (quoteTwo is null)
            return new List<CompareQuotesDifferentItem> { new CompareQuotesDifferentItem("quote", "notNull", "null") };

        var result = new List<CompareQuotesDifferentItem>();

        foreach (var propertyInfo in typeof(QuoteModel).GetProperties())
        {
            var valueOne = propertyInfo.GetValue(quoteOne);
            var valueTwo = propertyInfo.GetValue(quoteTwo);

            var isDifferent = valueOne is decimal valueOneD && valueTwo is decimal valueTwoD
                ? Math.Abs(valueOneD - valueTwoD) > Math.Max(valueOneD, valueTwoD) * allowableDifference / 100
                : valueOne?.Equals(valueTwo) != true;

            if (isDifferent)
                result.Add(new CompareQuotesDifferentItem(propertyInfo.Name, valueOne?.ToString() ?? string.Empty, valueTwo?.ToString() ?? string.Empty));
        }

        return result;
    }
}