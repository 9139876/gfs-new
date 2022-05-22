using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFS.WebApplication
{
    public abstract class CommonStartup
    {
        protected abstract void ConfigureServiceCollection(IServiceCollection services);
        protected abstract void ConfigureMapper(IServiceCollection services);

        //protected abstract void ConfigureServiceCollections(IServiceCollection services);

        protected CommonStartup()
        {
            Configuration = new ConfigurationBuilder()
                .Build();
        }

        public static IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            });

            //services.RegisterRemoteCall();

            services.AddMvc();

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                var assemblyInfo = AssemblyInfo.GetAssemblyInfo();

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = assemblyInfo.Version,
                    Title = assemblyInfo.Title,
                    Description = assemblyInfo.Description
                });
            });

            //services.AddRazorPages().AddRazorRuntimeCompilation();


            ConfigureServiceCollection(services);
            ConfigureMapper(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (_allowCORS)
            //{
            //    app.UseCorsMiddleware();
            //}

            //ConfigureApplication(app, env);
        }
    }
}
