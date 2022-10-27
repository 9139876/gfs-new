namespace GFS.BkgWorker;

public interface IBkgTask
{
    Task Execute(BkgTaskContext context);
}
