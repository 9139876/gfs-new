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

    public void ReportOfSuccessIteration()
    {
        _attemptLeft = AttemptsDefault;
        State = TaskStateEnum.InQueue;
    }

    public void ReportOfFailIteration()
    {
        _attemptLeft--;
        State = _attemptLeft > 0 ? TaskStateEnum.ReQueuedAfterError : TaskStateEnum.Failed;
    }

    public void SetAttempts(byte attempts)
    {
        AttemptsDefault = attempts;
        _attemptLeft = AttemptsDefault;
    }

    public void SetInQueueState()
    {
        State = TaskStateEnum.InQueue;
    }

    public void ReportOfComplete()
    {
        State = TaskStateEnum.Completed;
    }

    public void SetExecutingState()
    {
        State = TaskStateEnum.Executing;
    }

    public void SetCancelState()
    {
        State = TaskStateEnum.Canceled;
    }

    public string Serialize()
        => Context.Serialize();
}