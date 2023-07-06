using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;

#pragma warning disable CS8618

namespace GFS.WebApplication
{
    public abstract class CustomConfigurationActionsAbstract
    {
        public IServiceCollection ServiceCollection { protected get; init; }
        public IConfiguration Configuration { protected get; init; }
        public Microsoft.AspNetCore.Builder.WebApplication Application { protected get; set; }

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

        public LoggerConfiguration CustomConfigureLogger(LoggerConfiguration lc)
        {
            lc.Enrich.FromLogContext()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .Enrich.WithExceptionDetails()
                .WriteTo.Seq("http://192.168.1.100:5341")
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