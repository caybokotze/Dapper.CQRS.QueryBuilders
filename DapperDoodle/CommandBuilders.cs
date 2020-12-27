using System;
using System.Data;
using System.Text;
using DapperDoodle.Exceptions;

namespace DapperDoodle
{
    public static class CommandBuilders
    {
        public static string BuildInsertStatement<T>(this Command command)
        {
            return BuildInsertStatement<T>(command, null, Case.Lowercase);
        }
        
        public static string BuildInsertStatement<T>(this Command command, string table)
        {
            return BuildInsertStatement<T>(command, table, Case.Lowercase);
        }
        
        /// <summary>
        /// Override to specify the casing that should be used for the table and variable names.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="casing"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string BuildInsertStatement<T>(this Command command, Case casing)
        {
            return BuildInsertStatement<T>(command, null, casing);
        }
        
        /// <summary>
        /// Override for table to turn off the table pluralization.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="table"></param>
        /// <param name="casing"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
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
                default:
                    throw new InvalidDatabaseTypeException();
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
            sqlStatement.Append("); ");

            command.AppendReturnId(sqlStatement);
            
            return sqlStatement.ToString();
        }

        public static string BuildUpdateStatement<T>(this Command command, string whereClause = null)
        {
            return BuildUpdateStatement<T>(command, Case.Lowercase, whereClause);
        }

        public static string BuildUpdateStatement<T>(this Command command, Case casing, string whereClause = null)
        {
            return BuildUpdateStatement<T>(command, null, casing, whereClause);
        }
        
        public static string BuildUpdateStatement<T>(this Command command, string table, Case casing, string whereClause = null)
        {
            var dt = typeof(T).ObjectToDataTable();

            if (table is null)
                table = typeof(T).Name.Pluralize().ConvertCase(casing);

            var sqlStatement = new StringBuilder();

            switch (command.Dbms)
            {
                case DBMS.SQLite:
                    sqlStatement.Append($"UPDATE {table}");
                    break;
                default:
                    throw new InvalidDatabaseTypeException();
            }

            foreach (DataColumn column in dt.Columns)
            {
                sqlStatement.Append($"SET {column.ColumnName.ConvertCase(casing)} = @{column.ColumnName}, ");
            }

            sqlStatement.Remove(sqlStatement.Length - 2, 2);

            if (whereClause is null)
                whereClause = "WHERE id = @Id";

            sqlStatement.Append(whereClause);
            
            return sqlStatement.ToString();
        }

        public static string BuildDeleteStatement<T>(this Command command, string table, Case casing, string whereClause = null)
        {
            if (whereClause is null)
                whereClause = "WHERE id = @Id";
            
            var sqlStatement = new StringBuilder();

            switch (command.Dbms)
            {
                case DBMS.SQLite:
                    sqlStatement.Append($"DELETE FROM {table}");
                    break;
                default:
                    throw new InvalidDatabaseTypeException();
            }

            sqlStatement.Append(whereClause);

            return sqlStatement.ToString();
        }
    }
}