using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Windows.Input;
using DapperDiddle.Commands;
using Org.BouncyCastle.Asn1.X509.Qualified;

namespace DapperDiddle
{
    public static class CommandBuilders
    {
        public static string BuildInsert<T>(this Command command, string table = null)
        {
            var dt = typeof(T).ObjectToDataTable();

            foreach (DataColumn item in dt.Columns)
            {
                Console.WriteLine(item.DataType);
            }
            
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
            var props = TypeDescriptor.GetProperties(obj);
            var dt = new DataTable();

            foreach (PropertyDescriptor p in props)
            {
                dt.Columns.Add(p.Name, p.PropertyType);
            }

            return dt;
        }
    }
}