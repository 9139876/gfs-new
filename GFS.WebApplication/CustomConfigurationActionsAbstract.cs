using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;

#pragma warning disable CS8618

namespace GFS.WebApplication
{
    public abstract class CustomConfigurationActionsAbstract
    {
        public LoggerConfiguration CustomConfigureLogger(LoggerConfiguration lc)
        {
            lc.Enrich.FromLogContext()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
                .Enrich.WithExceptionDetails()
                // .WriteTo.Seq("http://192.168.1.100:5341")
                .WriteTo.Console()
                .WriteTo.Debug();

            return CustomConfigureLoggerInternal(lc);
        }

        /// <summary>
        /// Конфигурация логгера
        /// </summary>
        /// <param name="lc">Конфигуратор логгера</param>
        protected virtual LoggerConfiguration CustomConfigureLoggerInternal(LoggerConfiguration lc)
        {
            return lc;
        }
    }

    /// <summary>
    /// Конфигурирование консольного приложения
    /// </summary>
    public abstract class ConsoleCustomConfigurationActionsAbstract : CustomConfigurationActionsAbstract
    {
        /// <summary>
        /// Регистрация специфичных сервисов (BL, DbContext)
        /// </summary>
        public void ConfigureServiceCollection(HostBuilderContext ctx, IServiceCollection serviceCollection)
        {
            ConfigureServiceCollectionInternal(serviceCollection, ctx.Configuration);
            ConfigureMapper(serviceCollection, ctx.Configuration);
        }

        protected abstract void ConfigureServiceCollectionInternal(IServiceCollection serviceCollection, IConfiguration configuration);
        
        protected virtual void ConfigureMapper(IServiceCollection serviceCollection, IConfiguration configuration)
        {
        }
    }

    /// <summary>
    /// Конфигурирование Web приложения
    /// </summary>
    public abstract class WebCustomConfigurationActionsAbstract : CustomConfigurationActionsAbstract
    {
        public IServiceCollection ServiceCollection { get; init; }
        public IConfiguration Configuration { protected get; init; }
        
        public Microsoft.AspNetCore.Builder.WebApplication Application { protected get; set; }

        public virtual Action<MvcOptions> ConfigureControllers => _ => { };

        /// <summary>
        /// Регистрация специфичных сервисов (BL, DbContext)
        /// </summary>
        public abstract void ConfigureServiceCollection();

        /// <summary>
        /// Конфигурация маппера
        /// </summary>
        public virtual void ConfigureMapper()
        {
        }
        
        /// <summary>
        /// Действия после конфигурации до запуска (миграции, seeds)
        /// </summary>
        public virtual Task ConfigureApplication()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Выполнится после запуска приложения
        /// </summary>
        public virtual void OnApplicationStarted()
        {
        }

        /// <summary>
        /// Выполнится перед остановкой приложения
        /// </summary>
        public virtual void OnApplicationStopping()
        {
        }

        /// <summary>
        /// Выполнится после остановки приложения
        /// </summary>
        public virtual void OnApplicationStopped()
        {
        }
    }
}