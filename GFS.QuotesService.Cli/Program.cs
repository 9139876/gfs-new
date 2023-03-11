using GFS.Api.Client.Extensions;
using GFS.QuotesService.Cli.Pages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

namespace GFS.QuotesService.Cli;

public static class Program
{
    public static IServiceProvider ServiceProvider { get; }

    static Program()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(path: "config.json", optional: false);

        IConfiguration config = builder.Build();

        ServiceProvider = new ServiceCollection()
            .RegisterRemoteApi()
            .AddSingleton(config)
            .BuildServiceProvider();
    }

    public static int Main(string[] args)
    {
        AnsiConsole.Background = Color.Black;
        AnsiConsole.Foreground = Color.White;

        new RootPage().Show();
        return 0;
    }
}