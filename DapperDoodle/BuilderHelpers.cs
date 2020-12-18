using System;
using System.ComponentModel;
using System.Data;
using System.Text;
using PluralizeService.Core;

namespace DapperDoodle
{
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

        public static string AppendReturnId(
            this BaseSqlExecutor executor, 
            StringBuilder builder)
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

        public static string AppendReturnId(
            this BaseSqlExecutor executor, 
            string originalString)
        {
            var builder = new StringBuilder(originalString);
            return AppendReturnId(executor, builder);
        }
    }
}