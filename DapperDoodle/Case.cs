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

                    return value.ToKebabCase();
                }
                case Case.SnakeCase:
                {
                    if (value is null)
                    {
                        throw new ArgumentNullException(nameof(value));
                    }

                    return value.ToSnakeCase();
                }
            }

            return value;
        }
    }
}