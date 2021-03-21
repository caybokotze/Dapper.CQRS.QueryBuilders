using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using PeanutButter.Utils;

namespace DapperDoodle
{
    public enum Case
    {
        IgnoreCase,
        Uppercase,
        PascalCase,
        Lowercase,
        CamelCase,
        KebabCase,
        SnakeCase
    }
    
    public static class CaseHelpers
    {
        /// <summary>
        /// The ConvertCase method currently only supports conversions from Pascal Case to everything else. Other support can later be added.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="casing"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string ConvertCase(this string value, Case casing)
        {
            switch (casing)
            {
                case Case.Lowercase:
                    return value.ToLowerInvariant();
                case Case.Uppercase:
                    return value.ToUpperInvariant();
                case Case.CamelCase:
                    return char.ToLowerInvariant(value[0]) + value.Substring(1);
                case Case.PascalCase:
                {
                    var textInfo = new CultureInfo("en-Us", false).TextInfo;
                    return textInfo
                        .ToString()
                        .ToPascalCase();
                }
                case Case.IgnoreCase:
                    return value;
                case Case.KebabCase:
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        return value;
                    }
                    return Regex.Replace(value,
                            "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])",
                            "-$1")
                        .Trim()
                        .ToLower();
                }
                case Case.SnakeCase:
                {
                    if(value is null)
                        throw new ArgumentNullException(nameof(value));
                    if (value.Length < 2)
                        return value;
                    var sb = new StringBuilder();
                    sb.Append(char.ToLowerInvariant(value[0]));
                    for (int i = 1; i < value.Length; ++i)
                    {
                        char c = value[i];
                        if (char.IsUpper(c))
                        {
                            sb.Append('_');
                            sb.Append(char.ToLowerInvariant(c));
                        }
                        else
                        {
                            sb.Append(c);
                        }
                    }

                    return sb.ToString();
                }
            }

            return value;
        }
    }
}