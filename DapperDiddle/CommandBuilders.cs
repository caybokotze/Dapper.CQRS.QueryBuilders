using System;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Input;
using DapperDiddle.Commands;
using Org.BouncyCastle.Asn1.X509.Qualified;
using PluralizeService.Core;

namespace DapperDiddle
{
    public static class CommandBuilders
    {
        public static string BuildInsert<T>(this Command command)
        {
            return BuildInsert<T>(command, null, Case.Lowercase);
        }
        
        public static string BuildInsert<T>(this Command command, Case casing)
        {
            return BuildInsert<T>(command, null, casing);
        }
        
        private static string BuildInsert<T>(this Command command, string table, Case casing)
        {
            var dt = typeof(T).ObjectToDataTable();

            foreach (DataColumn item in dt.Columns)
            {
                Console.WriteLine(item.DataType);
            }

            if (table is null)
                table = typeof(T).Name.Pluralize().ConvertCase(Case.Lowercase);
            
            var sql = new StringBuilder($"INSERT INTO {table}");
            
            
            
            return "";
        }

        public static string BuildUpdate<T>(this T type)
        {
            return "";
        }

        public static string BuildSelect<T>(this T type)
        {
            return "";
        }
    }

    public static class BuilderHelpers
    {
        public static DataTable ObjectToDataTable(this Type obj)
        {
            var dt = new DataTable();
            var props = TypeDescriptor.GetProperties(obj);
            
            foreach (PropertyDescriptor p in props)
            {
                dt.Columns.Add(p.Name, p.PropertyType);
            }

            return dt;
        }

        public static string Pluralize(this string value)
        {
            return PluralizationProvider.Pluralize(value);
        }
    }
}