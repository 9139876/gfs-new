namespace GFS.QuotesService.Api.Common.Enum;

/// <summary>
/// Типы заданий на получение котировок у провайдера
/// </summary>
public enum GetQuotesTaskTypeEnum
{
    /// <summary>
    /// Инициировать список инструментов провайдера
    /// </summary>
    GetInitialData = 1,

    /// <summary>
    /// Загрузить историю котировок по инструменту
    /// </summary>
    GetHistory = 2,
    
    /// <summary>
    /// Получать котировки в реальном времени
    /// </summary>
    GetRealtimeQuotes = 3
}