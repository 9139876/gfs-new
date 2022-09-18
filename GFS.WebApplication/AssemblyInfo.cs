using System.Reflection;

namespace GFS.WebApplication
{
    public class AssemblyInfo
    {
        public string Title { get; private set; } = "Unknown";

        public string Description { get; private set; } = string.Empty;

        public string Version { get; private set; } = "v1";

        public static AssemblyInfo GetAssemblyInfo() => GetAssemblyInfo(Assembly.GetEntryAssembly());

        public static AssemblyInfo GetAssemblyInfo(Assembly? assembly)
        {
            var assemblyInfo = new AssemblyInfo();

            assemblyInfo.Title = GetAssemblyAttribute<AssemblyTitleAttribute>(assembly)?.Title ?? assemblyInfo.Title;
            assemblyInfo.Description = GetAssemblyAttribute<AssemblyDescriptionAttribute>(assembly)?.Description ?? assemblyInfo.Description;
            assemblyInfo.Version = assembly?.GetName()?.Version?.ToString() ?? assemblyInfo.Version;

            return assemblyInfo;
        }

        private static T? GetAssemblyAttribute<T>(ICustomAttributeProvider? assembly) where T : Attribute
            => assembly?.GetCustomAttributes(typeof(T), true)?.FirstOrDefault() as T;
    }
}