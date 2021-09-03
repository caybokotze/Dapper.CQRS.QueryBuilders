using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using Dapper.CQRS.Exceptions;

namespace Dapper.CQRS.QueryBuilders
{
    public static class QueryBuilders
    {
        public static string BuildSelectStatement<T>(this Query query)
        {
            return BuildSelectStatement<T>(query, null, Case.Lowercase);
        }
        
        public static string BuildSelectStatement<T>(this Query query, string table, string clause)
        {
            return BuildSelectStatement<T>(query, table, Case.Lowercase, clause);
        }
        
        public static string BuildSelectStatement<T>(this Query query, Case @case)
        {
            return BuildSelectStatement<T>(query, null, @case);
        }
        
        public static string BuildSelectStatement<T>(this Query query, string table, Case casing, string clause = null, object removeParameters = null)
        {
            var dt = typeof(T).DataTableForType();

            table ??= typeof(T).Name.Pluralize().ConvertCase(casing);
            
            var sqlStatement = new StringBuilder();
            
            var variables = new StringBuilder();

            if (removeParameters != null)
            {
                var props = removeParameters.GetObjectProperties();
                dt.RemovePropertiesFromDatatable(props);
            }

            foreach (DataColumn column in dt.Columns)
            {
                variables.Append($"{column.ColumnName.ConvertCase(casing)}, ");
            }

            variables.Remove(variables.Length - 2, 2);

            switch (query.Dbms)
            {
                case DBMS.MySQL:
                    sqlStatement.Append($"SELECT {variables} FROM {table}");
                    break;
                case DBMS.SQLite:
                    sqlStatement.Append($"SELECT {variables} FROM `{table}`");
                    break;
                case DBMS.MSSQL:
                    sqlStatement.Append($"SELECT {variables} FROM [{table}]");
                    break;
                default:
                    throw new InvalidDatabaseTypeException();
            }

            if (clause != null)
            {
                sqlStatement.Append(" ");
                sqlStatement.Append(clause);
            }
            
            sqlStatement.Append(";");

            return sqlStatement.ToString();
        }
    }
}