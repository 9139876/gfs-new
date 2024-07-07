using GFS.GrailCommon.Enums;
using GFS.QuotesService.DAL.Entities;

#pragma warning disable CS8618

namespace GFS.QuotesService.BL.Models.Adapters;

/// <summary>
/// Модель запроса адаптера котировок самой первой котировки
/// </summary>
internal class GetFirstQuoteAdapterRequestModel
{
    /// <summary>
    /// Актив
    /// </summary>
    public AssetEntity Asset { get; init; }
    
    /// <summary>
    /// Таймфрейм
    /// </summary>
    public TimeFrameEnum TimeFrame { get; init; }
}