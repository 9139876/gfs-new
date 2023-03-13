using GFS.WebApplication;

namespace GFS.Portfolio.WebApp
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            await ProgramUtils.RunWebHost<CustomConfigurationActions>(args);
        }
    }    
}

