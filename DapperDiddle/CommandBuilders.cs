using System;
using System.ComponentModel;
using System.Data;
using System.Text;
using DapperDiddle.Commands;
using PluralizeService.Core;

namespace DapperDiddle
{
    public static class CommandBuilders
    {
        public static string BuildInsertStatement<T>(this Command command)
        {
            return BuildInsertStatement<T>(command, null, Case.Lowercase);
        }
        
        public static string BuildInsertStatement<T>(this Command command, Case casing)
        {
            return BuildInsertStatement<T>(command, null, casing);
        }
        
        private static string BuildInsertStatement<T>(this Command command, string table, Case casing)
        {
            var dt = typeof(T).ObjectToDataTable();

            if (table is null)
                table = typeof(T).Name.Pluralize().ConvertCase(casing);
            
            var sql = new StringBuilder($"INSERT INTO {table} (");
            
            foreach (DataColumn column in dt.Columns)
            {
                sql.Append($"{column.ColumnName.ConvertCase(casing)}, ");
            }

            sql.Remove(sql.Length -2, 2);
            sql.Append(") VALUES (");

            foreach (DataColumn column in dt.Columns)
            {
                sql.Append($"@{column.ColumnName}, ");
            }

            sql.Remove(sql.Length - 2, 2);
            sql.Append(");");

            return sql.ToString();
        }

        public static string BuildUpdateStatement<T>(this T type)
        {
            return "";
        }

        public static string BuildSelectStatement<T>(this T type)
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