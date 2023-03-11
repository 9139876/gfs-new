namespace GFS.Common.Extensions;

public static class StringExtensions
{
    public static string ToCamelCaseFirstCharacterLower(this string str)
        => $"{str[0].ToString().ToLower()}{str.Substring(1, str.Length - 1)}";
}