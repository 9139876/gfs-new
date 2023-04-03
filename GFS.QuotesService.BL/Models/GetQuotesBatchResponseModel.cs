using GFS.GrailCommon.Models;

namespace GFS.QuotesService.BL.Models;

/// <summary>
/// Модель ответа адаптера котировок на запрос получения партии котировок
/// </summary>
public class GetQuotesBatchResponseModel
{
    /// <summary>
    /// Котировки
    /// </summary>
    public List<QuoteModel> Quotes { get; init; } = new();

    /// <summary>
    /// Признак, что больше котировок нет
    /// </summary>
    public bool IsLastBatch => NextBatchBeginningDate == null;
    
    /// <summary>
    /// Стартовая дата следующей партии
    /// </summary>
    public DateTime? NextBatchBeginningDate { get; init; }
}