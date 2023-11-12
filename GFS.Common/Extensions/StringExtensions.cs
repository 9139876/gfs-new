namespace GFS.Common.Extensions;

public static class StringExtensions
{
    public static string ToCamelCaseFirstCharacterLower(this string str)
        => $"{str[0].ToString().ToLower()}{str.Substring(1, str.Length - 1)}";

    public static bool IsEmpty(this string? str)
        => string.IsNullOrEmpty(str);
    
    public static bool IsNotEmpty(this string? str)
        => !string.IsNullOrEmpty(str);
    
    public static bool IsEmptyOrWhiteSpace(this string? str)
        => string.IsNullOrWhiteSpace(str);
    
    public static bool IsNotEmptyOrWhiteSpace(this string? str)
        => !string.IsNullOrWhiteSpace(str);
}