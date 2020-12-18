using System.Globalization;

namespace DapperDoodle
{
    public enum Case
    {
        IgnoreCase,
        Uppercase,
        PascalCase,
        Lowercase,
        CamelCase
    }
    
    public static class CaseHelpers
    {
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
                    TextInfo textInfo = new CultureInfo("en-Us", false).TextInfo;
                    return textInfo.ToTitleCase(value);
                }
                case Case.IgnoreCase:
                    return value;
            }

            return value;
        }
    }
}