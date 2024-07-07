namespace GFS.QuotesService.BL.Models.Adapters;

/// <summary>
/// Модель запроса адаптера котировок на получение партии котировок
/// </summary>
internal class GetQuotesBatchAdapterRequestModel : GetFirstQuoteAdapterRequestModel
{
    /// <summary>
    /// Дата первой котировки в партии
    /// </summary>
    public DateTime BatchBeginningDate { get; set; }
}