using GFS.WebApplication;

namespace TestWebApp2
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            await ProgramUtils.RunWebhost<CustomConfigurationActions>(args);
        }
    }
}