using GFS.WebApplication;

namespace GFS.QuotesService.Scheduler
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            await ProgramUtilsNew.RunWebhost<CustomConfigurationActions>(args);
        }
    }    
}