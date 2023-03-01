using GFS.Api.Client.Extensions;
using GFS.Api.Client.Services;
using GFS.QuotesService.Cli.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace GFS.QuotesService.Cli;

public static class Program
{
    public static Configuration Configuration { get; private set; } = new();
    
    public static IServiceProvider ServiceProvider { get; private set; } 

    public static int Main(string[] args)
    {
        if (args.FirstOrDefault() == "fac")
        {
            ForAutoComplete();
            return 0;
        }

        try
        {
            var app = new CommandApp();

            app.Configure(ConfigureCommands);
            Configure();

            return app.Run(args);
        }
        catch (Exception ex)
        {
            while (ex.InnerException != null)
                ex = ex.InnerException;

            Console.WriteLine(ex.Message);
            return ex.HResult;
        }
    }

    private static void ConfigureCommands(IConfigurator configurator)
    {
        configurator.AddCommand<AddTasksCommand>("addTasks")
            .WithDescription("Добавление задач в очередь");

        configurator.AddCommand<CancelTasksCommand>("cancelTasks")
            .WithDescription("Отмена всех ожидающих в очереди задач");

        configurator.AddCommand<GetTasksCommand>("getTasks")
            .WithDescription("Получение состояния выполняемых задач");

        configurator.AddCommand<GetAssetQuotesInfoCommand>("getAssetQuotesInfo")
            .WithDescription("Получение информации о загруженных котировках по инструменту");

        configurator.AddCommand<GetAssetsInfoCommand>("getAssetsInfo")
            .WithDescription("Получение информации по инструментам");
    }

    private static void Configure()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(path: "config.json", optional: false);

        IConfiguration config = builder.Build();

        Configuration = config.Get<Configuration>();
        
        Console.WriteLine($"{nameof(Configuration.QuotesManagerApiUrl)}: {Configuration.QuotesManagerApiUrl}");
        Console.WriteLine($"{nameof(Configuration.QuotesManagerWorkerUrl)}: {Configuration.QuotesManagerWorkerUrl}");
        Console.WriteLine($"{nameof(Configuration.QuotesInfoFilesPath)}: {Configuration.QuotesInfoFilesPath}");
        Console.WriteLine($"{nameof(Configuration.SaveQuotesInfoToFile)}: {Configuration.SaveQuotesInfoToFile}");
        
        ServiceProvider = new ServiceCollection()
            .RegisterRemoteApi()
            .AddSingleton(config)
            .BuildServiceProvider();

    }

    private static void ForAutoComplete()
        => Console.WriteLine("getTasks getQuotesInfo");
}

public class Configuration
{
    private readonly string _quotesManagerApiUrl = string.Empty;
    private readonly string _quotesManagerWorkerUrl = string.Empty;
    private readonly string _quotesInfoFilesPath = string.Empty;

    public string QuotesManagerApiUrl
    {
        get => _quotesManagerApiUrl;
        init
        {
            if (!Uri.TryCreate(value, UriKind.Absolute, out _))
                throw new InvalidOperationException($"Configuration load failed - {nameof(QuotesManagerApiUrl)} is not valid url");

            _quotesManagerApiUrl = value;
        }
    }

    public string QuotesManagerWorkerUrl
    {
        get => _quotesManagerWorkerUrl;
        init
        {
            if (!Uri.TryCreate(value, UriKind.Absolute, out _))
                throw new InvalidOperationException($"Configuration load failed - {nameof(QuotesManagerWorkerUrl)} is not valid url");

            _quotesManagerWorkerUrl = value;
        }
    }

    public string QuotesInfoFilesPath
    {
        get => _quotesInfoFilesPath;
        init
        {
            if (!Directory.Exists(value))
                throw new InvalidOperationException($"Configuration load failed - directory {nameof(QuotesInfoFilesPath)} not exist");

            _quotesInfoFilesPath = value;
        }
    }

    public bool SaveQuotesInfoToFile { get; init; }
}