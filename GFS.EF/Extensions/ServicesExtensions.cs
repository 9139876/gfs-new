using System;
using System.Threading.Tasks;
using GFS.Common.Extensions;
using GFS.EF.Repository;
using GFS.EF.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GFS.EF.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection RegisterDbContext<TDbContext>(this IServiceCollection services, string connectionString)
            where TDbContext : DbContext, IRepositoryCollection
        {
            services.AddDbContext<TDbContext>(options => options.UseNpgsql(connectionString));
            services.AddScoped<IRepositoryCollection>(provider => provider.GetRequiredService<TDbContext>());
            services.AddScoped<DbContext>(provider => provider.GetRequiredService<TDbContext>());
            
            return services;
        }

        public static async Task MigrateDatabaseAsync<TDbContext>(this IServiceProvider serviceProvider)
            where TDbContext : DbContext
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;

            var context = services.GetService<TDbContext>();
            context.ThrowIfNull(new InvalidOperationException("DbContext is not specified"));

            var canConnect = await context.Database.CanConnectAsync();
            canConnect.ThrowIfFalse(new ArgumentOutOfRangeException(nameof(context.Database), "Database connection"));

            var logger = serviceProvider.GetRequiredService<ILogger>();
            logger.Log(LogLevel.Information, "Starting migration...");
            await context.Database.MigrateAsync();
            logger.Log(LogLevel.Information, "Migration finished");
        }
        
        // private static async Task MigrateDatabaseAsync<TDbContext, TSeeder>(IServiceProvider serviceProvider)
        //     where TDbContext : DbContext
        //     where TSeeder : class, ISeeder
        // {
        //     using var scope = serviceProvider.CreateScope();
        //     var services = scope.ServiceProvider;
        //
        //     var context = services.GetService<TDbContext>();
        //     context.ThrowIfNull(new InvalidOperationException("DbContext is not specified"));
        //
        //     var canConnect = await context.Database.CanConnectAsync();
        //     canConnect.ThrowIfFalse(new ArgumentOutOfRangeException(nameof(context.Database), "Database connection"));
        //
        //     //var configuration = services.GetService<IConfiguration>();
        //     //var applyMigration = !string.IsNullOrEmpty(configuration.GetValue<string>("ASPNETCORE_INPLACE_MIGRATION"));
        //     Log.Information("Starting inplace migration...");
        //     await context.Database.MigrateAsync();
        //
        //     await Seeding<TDbContext, TSeeder>(context);
        // }
    }
}