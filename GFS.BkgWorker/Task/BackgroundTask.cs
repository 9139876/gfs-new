using GFS.BkgWorker.Enum;

namespace GFS.BkgWorker.Task;

public class BackgroundTask
{
    private byte _attemptLeft;

    public BackgroundTask(TaskContext context, byte attempts = 3)
    {
        Context = context;
        AttemptsDefault = attempts;
        _attemptLeft = AttemptsDefault;
    }

    public TaskPriorityEnum Priority { get; set; }
    
    public uint IterationNumber { get; set; } 
    
    public byte AttemptsDefault { get; private set; }

    public TaskContext Context { get; }

    public TaskStateEnum State { get; private set; }
    
    public string? LastError { get; private set; }

    public void ReportOfSuccessIteration()
    {
        _attemptLeft = AttemptsDefault;
        LastError = null;
        State = TaskStateEnum.PendingExecution;
    }

    public void ReportOfFailIteration(string error)
    {
        _attemptLeft--;
        LastError = error;
        // State = _attemptLeft > 0 ? TaskStateEnum.ReQueuedAfterError : TaskStateEnum.Failed;
    }

    public void SetAttempts(byte attempts)
    {
        AttemptsDefault = attempts;
        _attemptLeft = AttemptsDefault;
    }

    public void SetInQueueState()
    {
        LastError = null;
        State = TaskStateEnum.PendingExecution;
    }

    public void ReportOfComplete()
    {
        LastError = null;
        State = TaskStateEnum.Completed;
    }

    public void SetExecutingState()
    {
        LastError = null;
        State = TaskStateEnum.Executing;
    }

    public void SetCancelState()
    {
        LastError = null;
        State = TaskStateEnum.Canceled;
    }

    public string Serialize()
        => Context.Serialize();
}