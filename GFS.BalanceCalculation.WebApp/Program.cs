using GFS.WebApplication;

namespace GFS.BalanceCalculation.WebApp
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            await ProgramUtilsNew.RunWebhost<CustomConfigurationActions>(args);
        }
    }    
}