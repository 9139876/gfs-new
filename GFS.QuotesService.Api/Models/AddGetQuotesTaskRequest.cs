using GFS.GrailCommon.Enums;
using GFS.QuotesService.Common.Enum;

namespace GFS.QuotesService.Api.Models;

public class AddGetQuotesTaskRequest
{
    /// <summary>
    /// Тип провайдера котировок
    /// </summary>
    public QuotesProviderTypeEnum QuotesProviderType { get; init; }
    
    /// <summary>
    /// Идентификатор актива
    /// </summary>
    public Guid AssetId { get; init; }
    
    /// <summary>
    /// Таймфрейм
    /// </summary>
    public TimeFrameEnum TimeFrame { get; init; }
}