using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
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

        /// <summary>
        /// Конфигурация логгера
        /// </summary>
        /// <param name="lc">Конфигуратор логгера</param>
        public virtual LoggerConfiguration CustomConfigureLogger(LoggerConfiguration lc)
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