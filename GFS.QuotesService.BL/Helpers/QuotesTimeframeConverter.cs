using GFS.GrailCommon.Enums;
using GFS.GrailCommon.Extensions;
using GFS.GrailCommon.Models;

namespace GFS.QuotesService.BL.Helpers;

internal static class QuotesTimeframeConverter
{
    public static List<QuoteModel> ConvertToTimeframe(List<QuoteModel> quotes, TimeFrameEnum sourceTf, TimeFrameEnum targetTf)
    {
        if (targetTf <= sourceTf)
            throw new InvalidOperationException($"Попытка преобразовать из {sourceTf} в {targetTf} - а возможно только преобразование из младшего в старший");

        return quotes
            .Select(q => new { Date = q.Date.CorrectDateByTf(targetTf), Quote = q })
            .GroupBy(x => x.Date)
            .OrderBy(x => x.Key)
            .Select(x => new QuoteModel
            {
                TimeFrame = targetTf,
                Date = x.Key,
                Open = x.OrderBy(item => item.Quote.Date).First().Quote.Open,
                High = x.Max(item => item.Quote.High),
                Low = x.Min(item => item.Quote.Low),
                Close = x.OrderBy(item => item.Quote.Date).Last().Quote.Close,
                Volume = x.Sum(item => item.Quote.Volume)
            })
            .ToList();
    }
}