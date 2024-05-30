using GFS.WebApplication;

namespace TestWebApp1
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            await ProgramUtils.RunWebHost<WebCustomConfigurationActions>(args);
        }
    }
}