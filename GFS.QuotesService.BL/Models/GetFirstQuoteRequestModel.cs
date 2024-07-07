using GFS.GrailCommon.Enums;
using GFS.QuotesService.Common.Enum;
using GFS.QuotesService.DAL.Entities;
#pragma warning disable CS8618

namespace GFS.QuotesService.BL.Models;

/// <summary>
/// Модель запроса самой первой котировки
/// </summary>
public class GetFirstQuoteRequestModel
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
}