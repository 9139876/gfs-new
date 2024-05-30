using GFS.WebApplication;

namespace GFS.Broker.WebApp
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            await ProgramUtils.RunWebHost<WebCustomConfigurationActions>(args);
        }
    }    
}