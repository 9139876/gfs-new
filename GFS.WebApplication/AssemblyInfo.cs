using System.Reflection;

namespace GFS.WebApplication
{
    public class AssemblyInfo
    {
        public string Title { get; set; } = "Unknown";

        public string Description { get; set; } = string.Empty;

        public string Version { get; set; } = "v1";

        public static AssemblyInfo GetAssemblyInfo() => GetAssemblyInfo(Assembly.GetEntryAssembly());

        public static AssemblyInfo GetAssemblyInfo(Assembly assembly)
        {
            var assemblyInfo = new AssemblyInfo();

            assemblyInfo.Title = GetAssemblyAttribute<AssemblyTitleAttribute>(assembly)?.Title ?? assemblyInfo.Title;
            assemblyInfo.Description = GetAssemblyAttribute<AssemblyDescriptionAttribute>(assembly)?.Description ?? assemblyInfo.Description;
            assemblyInfo.Version = assembly.GetName()?.Version?.ToString() ?? assemblyInfo.Version;

            return assemblyInfo;
        }

        private static T GetAssemblyAttribute<T>(Assembly assembly) where T : Attribute
        {
            var attributes = assembly?.GetCustomAttributes(typeof(T), true);

            if (attributes?.Length > 0)
                return (T)attributes[0];

            return null;
        }
    }
}
