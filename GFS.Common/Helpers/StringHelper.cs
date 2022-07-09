using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace GFS.Common.Helpers
{
    public static class StringHelper
    {
        /// <summary>
        /// Inspired by https://www.npmjs.com/package/common-tags#oneline
        /// </summary>
        public static string AsOneLine(this string text, string separator = " ") => new Regex(@"(?:\n(?:\s*))+").Replace(text, separator).Trim();

        /// <summary>
        /// Transform first character of a string into lower case
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FirstCharacterToLower(this string str)
        {
            if (string.IsNullOrEmpty(str) || char.IsLower(str, 0))
                return str;

            return char.ToLowerInvariant(str[0]) + str.Substring(1);
        }

        /// <summary>
        /// Capitalizes first letter of a string
        /// </summary>
        public static string Capitalize(this string word) => string.Concat(word.First().ToString().ToUpper(), word.AsSpan(1));

        public static bool IsValidJson(this string stringValue)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
                return false;

            var value = stringValue.Trim();

            if ((value.StartsWith("{") && value.EndsWith("}")) || //For object
                (value.StartsWith("[") && value.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(value);
                    return true;
                }
                catch (JsonReaderException)
                {
                    return false;
                }
            }

            return false;
        }

        public static string ToKebabCase(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return Regex.Replace(
                    value,
                    "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])",
                    "-$1",
                    RegexOptions.Compiled)
                .Trim()
                .ToLower();
        }
    }
}