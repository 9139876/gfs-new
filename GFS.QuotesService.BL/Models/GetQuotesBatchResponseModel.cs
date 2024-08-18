using GFS.QuotesService.DAL.Entities;
#pragma warning disable CS8618

namespace GFS.QuotesService.BL.Models;

/// <summary>
/// Модель ответа на запрос получения партии котировок
/// </summary>
public class GetQuotesBatchResponseModel
{
    public GetQuotesBatchResponseModel(List<QuoteEntity> quotes, bool isLastBatch, DateTime? nextBatchBeginningDate)
    {
        Quotes = quotes;
        IsLastBatch = isLastBatch;
        NextBatchBeginningDate = nextBatchBeginningDate;
    }

    /// <summary>
    /// Коллекция котировок
    /// </summary>
    public List<QuoteEntity> Quotes { get; }
    
    /// <summary>
    /// Признак, что это на данный момент последняя существующая партия котировок
    /// </summary>
    public bool IsLastBatch { get; }
    
    /// <summary>
    /// Дата начала следующей партии котировок
    /// </summary>
    public DateTime? NextBatchBeginningDate { get; }
}