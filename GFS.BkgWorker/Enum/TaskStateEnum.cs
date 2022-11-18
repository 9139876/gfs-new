namespace GFS.BkgWorker.Enum;

public enum TaskStateEnum
{
    /// <summary>
    /// В очереди
    /// </summary>
    InQueue = 1,
    
    /// <summary>
    /// Выполняется
    /// </summary>
    Executing = 2,
    
    /// <summary>
    /// Выполнена
    /// </summary>
    Completed = 3,
    
    /// <summary>
    /// Отменена
    /// </summary>
    Canceled = 4,
    
    /// <summary>
    /// Повторно поставлена в очередь после ошибки
    /// </summary>
    ReQueuedAfterError = 5,
    
    /// <summary>
    /// Число попыток исчерпано 
    /// </summary>
    Failed = 6
}