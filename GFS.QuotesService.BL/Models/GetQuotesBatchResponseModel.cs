using GFS.QuotesService.DAL.Entities;
#pragma warning disable CS8618

namespace GFS.QuotesService.BL.Models;

/// <summary>
/// Модель ответа на запрос получения партии котировок
/// </summary>
public class GetQuotesBatchResponseModel
{
    /// <summary>
    /// Коллекция котировок
    /// </summary>
    public List<QuoteEntity> Quotes { get; init; }
    
    /// <summary>
    /// Признак, что это на данный момент последняя существующая партия котировок
    /// </summary>
    public bool IsLastBatch { get; init; }
}