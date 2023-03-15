using GFS.WebApplication;

namespace GFS.TradingStrategyTester.BackgroundWorker;

public static class Program
{
    public static async Task Main(string[] args)
    {
        await ProgramUtils.RunWebHost<CustomConfigurationActions>(args);
    }
}