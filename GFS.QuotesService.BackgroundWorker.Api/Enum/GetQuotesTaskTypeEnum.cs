namespace GFS.QuotesService.BackgroundWorker.Api.Enum;

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
    /// Получать котировки в реальном времени
    /// </summary>
    GetRealtimeQuotes = 2,
    
    /// <summary>
    /// Загрузить историю котировок по инструменту
    /// </summary>
    GetHistory = 3
    
    
}