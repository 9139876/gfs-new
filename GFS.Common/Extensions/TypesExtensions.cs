namespace GFS.Common.Extensions
{
    public static class TypesExtensions
    {
        public static Type GetBaseClass(this Type type)
        {
            while (type.BaseType is {IsClass: true} && type.BaseType != typeof(object))
                type = type.BaseType;

            return type;
        }
    }
}