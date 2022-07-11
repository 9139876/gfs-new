using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using GFS.Common.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace GFS.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private static readonly List<string> _servicePostfix = new() {"Service"};

        public static void RegisterCurrentAssemblyServices(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped,
            bool optional = false)
        {
            RegisterClasses(services: services,
                assembly: Assembly.GetExecutingAssembly(),
                classesPostfix: _servicePostfix,
                serviceLifetime: serviceLifetime,
                optional: optional);
        }

        public static void RegisterAssemblyServices(this IServiceCollection services, string assemblyName, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped,
            bool optional = false)
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(x => x.GetName().Name == assemblyName);

            if (assembly == null)
                throw new InvalidOperationException($"Can't find assembly by Name = {assemblyName}");

            RegisterClasses(services: services,
                assembly: assembly,
                classesPostfix: _servicePostfix,
                serviceLifetime: serviceLifetime,
                optional: optional);
        }

        public static void RegisterAssemblyServicesByMember<T>(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped,
            bool optional = false)
        {
            RegisterClasses(services: services,
                assembly: typeof(T).Assembly,
                classesPostfix: _servicePostfix,
                serviceLifetime: serviceLifetime,
                optional: optional);
        }

        private static void RegisterClasses(IServiceCollection services, Assembly assembly, IReadOnlyCollection<string> classesPostfix, ServiceLifetime serviceLifetime,
            bool optional)
        {
            var implementTypes = assembly
                .GetTypes()
                .Where(t => classesPostfix.Any(x => t.Name.EndsWith(x))
                            && t.IsClass
                            && !t.GetCustomAttributes(typeof(IgnoreRegistrationAttribute), false).Any());

            foreach (var implementType in implementTypes)
            {
                var interfaceName = "I" + implementType.Name;
                var interfaceType = implementType.GetInterface(interfaceName);

                if (interfaceType == null)
                    if (optional == false)
                        throw new InvalidOperationException(
                            $"Can't find interface = {interfaceName} for class.Name = {implementType.Name}, class.FullName = {implementType.FullName}. Add 'IgnoreRegistrationAttribute' if need");
                    else
                        continue;

                var isSingleton = interfaceType.GetCustomAttributes(typeof(SingletonRegistrationAttribute), false).Any();
                services.Add(new ServiceDescriptor(interfaceType, implementType, isSingleton ? ServiceLifetime.Singleton : serviceLifetime));
            }
        }
    }
}