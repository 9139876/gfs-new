namespace GFS.QuotesService.Api.Models.CheckQuotes;

/// <summary>
/// Модель
/// </summary>
public class CompareTimeframesCheckQuotesRequest
{
    /// <summary>
    /// Идентификатор связки первого инструмента, его провайдера и таймфрейма
    /// </summary>
    public Guid AssetWithProviderOneId { get; init; }

    /// <summary>
    /// Идентификатор связки второго инструмента, его провайдера и таймфрейма
    /// </summary>
    public Guid AssetWithProviderTwoId { get; init; }

    /// <summary>
    /// Начальная дата
    /// </summary>
    public DateTime FirstDate { get; init; }

    /// <summary>
    /// Конечная дата
    /// </summary>
    public DateTime LastDate { get; init; }

    /// <summary>
    /// Допустимое расхождение (в процентах)
    /// </summary>
    public decimal AllowableDifferenceInPercents { get; init; }
}