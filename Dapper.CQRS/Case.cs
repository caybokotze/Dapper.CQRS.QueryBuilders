using System;
using PeanutButter.Utils;

namespace Dapper.CQRS
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
        /// <param name="case"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string ConvertCase(this string value, Case @case)
        {
            var
                context = new ConversionContext(new ConvertToUpperCase(value, @case));
            value = context.CallConvert();
            
            context = new ConversionContext(new ConvertToLowerCase(value, @case));
            value = context.CallConvert();
            
            context = new ConversionContext(new ConvertToCamelCase(value, @case));
            value = context.CallConvert();

            context = new ConversionContext(new ConvertToPascalCase(value, @case));
            value = context.CallConvert();

            context = new ConversionContext(new ConvertToKebabCase(value, @case));
            value = context.CallConvert();

            context = new ConversionContext(new ConvertToSnakeCase(value, @case));
            value = context.CallConvert();

            return value;
        }
    }

    internal abstract class MethodOfConversion
    {
        public abstract string Convert();
    }


    internal class ConvertToLowerCase : MethodOfConversion
    {
        private readonly string _value;
        private readonly Case _case;

        public ConvertToLowerCase(string value, Case @case)
        {
            _value = value;
            _case = @case;
        }
        
        public override string Convert()
        {
            return _case.Equals(Case.Lowercase) 
                ? _value.ToLowerInvariant() 
                : _value;
        }
    }

    internal class ConvertToUpperCase : MethodOfConversion
    {
        private readonly string _value;
        private readonly Case _case;

        public ConvertToUpperCase(string value, Case @case)
        {
            _value = value;
            _case = @case;
        }
        
        public override string Convert()
        {
            return _case.Equals(Case.Uppercase) 
                ? _value.ToUpperInvariant() 
                : _value;
        }
    }

    internal class ConvertToCamelCase : MethodOfConversion
    {
        private readonly string _value;
        private readonly Case _case;

        public ConvertToCamelCase(string value, Case @case)
        {
            _value = value;
            _case = @case;
        }
        
        public override string Convert()
        {
            return _case.Equals(Case.CamelCase)
                ? _value.ToCamelCase()
                : _value;
        }
    }

    internal class ConvertToPascalCase : MethodOfConversion
    {
        private readonly string _value;
        private readonly Case _case;

        public ConvertToPascalCase(string value, Case @case)
        {
            _value = value;
            _case = @case;
        }
        
        public override string Convert()
        {
            return _case.Equals(Case.PascalCase)
                ? _value.ToPascalCase()
                : _value;
        }
    }

    internal class ConvertToKebabCase : MethodOfConversion
    {
        private readonly string _value;
        private readonly Case _case;

        public ConvertToKebabCase(string value, Case @case)
        {
            _value = value;
            _case = @case;
        }
        
        public override string Convert()
        {
            return _case.Equals(Case.KebabCase)
                ? _value.ToKebabCase()
                : _value;
        }
    }

    internal class ConvertToSnakeCase : MethodOfConversion
    {
        private readonly string _value;
        private readonly Case _case;

        public ConvertToSnakeCase(string value, Case @case)
        {
            _value = value;
            _case = @case;
        }
        
        public override string Convert()
        {
            return _case.Equals(Case.SnakeCase)
                ? _value.ToSnakeCase()
                : _value;
        }
    }

    internal class ConversionContext
    {
        private MethodOfConversion MethodOfConversion { get; }

        public ConversionContext(MethodOfConversion methodOfConversion)
        {
            MethodOfConversion = methodOfConversion;
        }

        public string CallConvert()
        {
            return MethodOfConversion.Convert();
        }
    }
}