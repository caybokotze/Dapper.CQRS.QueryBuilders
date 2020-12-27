using System.Data;
using System.Text;
using DapperDoodle.Exceptions;

namespace DapperDoodle
{
    public static class QueryBuilders
    {
        public static string BuildSelectStatement<T>(this Query query)
        {
            return BuildSelectStatement<T>(query, null, Case.Lowercase);
        }
        
        public static string BuildSelectStatement<T>(this Query query, string table)
        {
            return BuildSelectStatement<T>(query, table, Case.Lowercase);
        }
        
        public static string BuildSelectStatement<T>(this Query query, string table, Case casing, string clause = null)
        {
            var dt = typeof(T).ObjectToDataTable();

            if (table is null)
                table = typeof(T).Name.Pluralize().ConvertCase(casing);
            
            var sqlStatement = new StringBuilder();
            
            var variables = new StringBuilder();

            foreach (DataColumn column in dt.Columns)
            {
                variables.Append($"{column.ColumnName},");
            }

            variables.Remove(variables.Length - 1, 1);

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

            sqlStatement.Append(clause);
            sqlStatement.Append(";");

            return sqlStatement.ToString();
        }
    }
}