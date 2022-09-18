using GFS.Common.Extensions;
using GFS.EF.Seed;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;

namespace GFS.WebApplication
{
    public static class ProgramUtils
    {
        // public static async Task RunWebhost<TStartup, TDbContext, TSeeder>(string[] args)
        public static async Task RunWebhost<TStartup>(string[] args)
            where TStartup : CommonStartup
            // where TDbContext : DbContext
            // where TSeeder : class, ISeeder
        {
            var config = new ConfigurationBuilder()
                //.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                //.AddEnvironmentVariables()
                .Build();
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .ReadFrom.Configuration(config)
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                //.WriteTo.Seq("http://seq:5341")
                .WriteTo.Console()
                .WriteTo.Debug()
                .CreateLogger();

            try
            {
                //@FixMe: application name
                Log.Information($"Start application");
                var webHost = CreateWebHostBuilder<TStartup>(args)
                    .Build();

                // Log.Information("Migration starting...");
                // //await Task.Run(async () => { await MigrateDatabaseAsync<TDbContext, TSeeder>(webHost); });
                // await MigrateDatabaseAsync<TDbContext, TSeeder>(webHost);
                // Log.Information("Migration finished");

                await webHost.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder<TStartup>(string[] args)
            where TStartup : class
        {
            //var builder = new WebHostBuilder();
            var builder = WebHost.CreateDefaultBuilder(args);

            builder.UseConfiguration(new ConfigurationBuilder().AddCommandLine(args).Build());

            builder
                .UseSerilog()
                .UseStartup<TStartup>();

            return builder;
        }

        private static async Task MigrateDatabaseAsync<TDbContext, TSeeder>(IWebHost webHost)
            where TDbContext : DbContext
            where TSeeder : class, ISeeder
        {
            using var scope = webHost.Services.CreateScope();
            var services = scope.ServiceProvider;

            var context = services.GetService<TDbContext>();
            context.ThrowIfNull(new InvalidOperationException("DbContext is not specified"));

            var canConnect = await context.Database.CanConnectAsync();
            canConnect.ThrowIfFalse(new ArgumentOutOfRangeException(nameof(context.Database), "Database connection"));

            var configuration = services.GetService<IConfiguration>();
            //var applyMigration = !string.IsNullOrEmpty(configuration.GetValue<string>("ASPNETCORE_INPLACE_MIGRATION"));
            var applyMigration = true;
            if (applyMigration)
            {
                Log.Information("Starting inplace migration...");
                await context.Database.MigrateAsync();
            }

            await Seeding<TDbContext, TSeeder>(context);
        }

        private static async Task Seeding<TDbContext, TSeeder>(TDbContext context)
            where TDbContext : DbContext
            where TSeeder : class, ISeeder
        {
            if (Activator.CreateInstance(typeof(TSeeder), context) is TSeeder seeder)
            {
                Log.Information("Seeding...");
                await seeder.Seed();
                Log.Information("Seeding is finished");
            }
            else
                Log.Information("Seeder is not specified.");
        }
    }
}