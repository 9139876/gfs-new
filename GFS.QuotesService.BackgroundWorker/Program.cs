using GFS.WebApplication;

namespace GFS.QuotesService.BackgroundWorker;

public static class Program
{
    public static async Task Main(string[] args)
    {
        await ProgramUtilsNew.RunWebhost<CustomConfigurationActions>(args);
    }
}