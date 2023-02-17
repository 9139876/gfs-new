namespace GFS.BkgWorker.Enum;

public enum TaskStateEnum
{
    /// <summary>
    /// Ожидает выполнения
    /// </summary>
    PendingExecution = 1,
    
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
    /// Прервана из-за ошибки
    /// </summary>
    Failed = 5
}