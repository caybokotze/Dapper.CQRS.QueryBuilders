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
                    return string.IsNullOrEmpty(value)
                        ? throw new NullReferenceException(nameof(value))
                        : value.ToCamelCase();
                case Case.PascalCase:
                {
                    return string.IsNullOrEmpty(value)
                        ? throw new NullReferenceException(nameof(value))
                        : value.ToPascalCase();
                }
                case Case.IgnoreCase:
                    return value;
                case Case.KebabCase:
                {
                    return string.IsNullOrEmpty(value)
                        ? throw new ArgumentNullException(nameof(value)) 
                        : value.ToKebabCase();
                }
                case Case.SnakeCase:
                {
                    return string.IsNullOrEmpty(value)
                        ? throw new ArgumentNullException(nameof(value))
                        : value.ToSnakeCase();
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(casing), casing, null);
            }
        }
    }
}