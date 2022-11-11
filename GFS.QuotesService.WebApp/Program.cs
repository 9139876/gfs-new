using GFS.WebApplication;

namespace GFS.QuotesService.WebApp
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            await ProgramUtils.RunWebhost<CustomConfigurationActions>(args);
        }
    }    
}