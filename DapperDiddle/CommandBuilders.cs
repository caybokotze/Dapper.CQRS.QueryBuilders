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
            
            var sqlStatement = new StringBuilder();

            switch (command.Dbms)
            {
                case DBMS.MySQL:
                    sqlStatement.Append($"INSERT INTO `{table}` (");
                    break;
                
                case DBMS.SQLite:
                    sqlStatement.Append($"INSERT INTO {table} (");
                    break;
                
                case DBMS.MSSQL:
                    sqlStatement.Append($"INSERT INTO [{table}] (");
                    break;
            }

            foreach (DataColumn column in dt.Columns)
            {
                sqlStatement.Append($"{column.ColumnName.ConvertCase(casing)}, ");
            }

            sqlStatement.Remove(sqlStatement.Length -2, 2);
            sqlStatement.Append(") VALUES (");

            foreach (DataColumn column in dt.Columns)
            {
                sqlStatement.Append($"@{column.ColumnName}, ");
            }

            sqlStatement.Remove(sqlStatement.Length - 2, 2);
            sqlStatement.Append(");");

            command.AppendReturnId(sqlStatement);
            
            return sqlStatement.ToString();
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

        public static string AppendReturnId(this BaseSqlExecutor executor, StringBuilder builder)
        {
            switch (executor.Dbms)
            {
                case DBMS.SQLite:
                    builder.Append("SELECT last_insert_rowid();");
                    break;
                
                case DBMS.MySQL:
                    builder.Append("SELECT LAST_INSERT_ID();");
                    break;
                
                case DBMS.MSSQL:
                    builder.Append("SELECT SCOPE_IDENTITY();");
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return builder.ToString();
        }

        public static string AppendReturnId(this BaseSqlExecutor executor, string originalString)
        {
            var builder = new StringBuilder(originalString);
            return AppendReturnId(executor, builder);
        }
    }
}