using GFS.GrailCommon.Enums;
using GFS.QuotesService.Api.Enum;
using GFS.QuotesService.DAL.Entities;
#pragma warning disable CS8618

namespace GFS.QuotesService.BL.Models;

/// <summary>
/// Модель запроса на получение партии котировок
/// </summary>
public class GetQuotesBatchRequestModel
{
    /// <summary>
    /// Тип провайдера котировок
    /// </summary>
    public QuotesProviderTypeEnum QuotesProviderType { get; init; }
    
    /// <summary>
    /// Актив
    /// </summary>
    public AssetEntity Asset { get; init; }
    
    /// <summary>
    /// Таймфрейм
    /// </summary>
    public TimeFrameEnum TimeFrame { get; init; }
    
    /// <summary>
    /// Дата последней загруженной котировки
    /// </summary>
    public DateTime LastQuoteDate { get; init; }
}