using System.Globalization;
using System.Text.RegularExpressions;

namespace DapperDoodle
{
    public enum Case
    {
        IgnoreCase,
        Uppercase,
        PascalCase,
        Lowercase,
        CamelCase,
        KebabCase
    }
    
    public static class CaseHelpers
    {
        public static string ConvertCase(this string value, Case casing)
        {
            switch (casing)
            {
                case Case.Lowercase:
                    return value.ToLowerInvariant();
                    break;
                case Case.Uppercase:
                    return value.ToUpperInvariant();
                    break;
                case Case.CamelCase:
                    return char.ToLowerInvariant(value[0]) + value.Substring(1);
                    break;
                case Case.PascalCase:
                {
                    var textInfo = new CultureInfo("en-Us", false).TextInfo;
                    return textInfo.ToTitleCase(value);
                    break;
                }
                case Case.IgnoreCase:
                    return value;
                    break;
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
                    break;
                }
            }

            return value;
        }
    }
}