using System;
using System.Data;
using System.Text;
using DapperDoodle.Exceptions;

namespace DapperDoodle
{
    public static class CommandBuilders
    {
        /// <summary>
        /// Default Insert Statement returns a SQL INSERT statement.
        /// </summary>
        /// <param name="command"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string BuildInsertStatement<T>(this Command command)
        {
            return BuildInsertStatement<T>(command, null, Case.Lowercase);
        }
        
        /// <summary>
        /// Table name override for when naming conventions do not match normal pluralization.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="table"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        
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
        /// Returns a string of the Insert Statement that will be inserted into the database.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="table"></param>
        /// <param name="casing"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string BuildInsertStatement<T>(this Command command, string table, Case casing)
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

        /// <summary>
        /// Returns an SQL UPDATE statement.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="whereClause"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string BuildUpdateStatement<T>(this Command command, string whereClause = null)
        {
            return BuildUpdateStatement<T>(command, Case.Lowercase, whereClause);
        }

        /// <summary>
        /// Returns a SQL UPDATE statement with the override for a where clause to specify the where condition.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="casing"></param>
        /// <param name="whereClause"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string BuildUpdateStatement<T>(this Command command, Case casing, string whereClause = null)
        {
            return BuildUpdateStatement<T>(command, null, casing, whereClause);
        }
        
        /// <summary>
        /// Returns a SQL UPDATE statement with the override for a where clause and an override for the case and table name.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="table"></param>
        /// <param name="casing"></param>
        /// <param name="whereClause"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="InvalidDatabaseTypeException"></exception>
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
                case DBMS.MySQL:
                    sqlStatement.Append($"UPDATE `{table}`");
                    break;
                case DBMS.MSSQL:
                    sqlStatement.Append($"UPDATE [{table}]");
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

        
        
        /// <summary>
        /// Returns a SQL DELETE statement.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="table"></param>
        /// <param name="casing"></param>
        /// <param name="whereClause"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="InvalidDatabaseTypeException"></exception>
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
                case DBMS.MySQL:
                    sqlStatement.Append($"DELETE FROM `{table}`");
                    break;
                case DBMS.MSSQL:
                    sqlStatement.Append($"DELETE FROM [{table}]");
                    break;
                default:
                    throw new InvalidDatabaseTypeException();
            }

            sqlStatement.Append(" ");
            sqlStatement.Append(whereClause);

            return sqlStatement.ToString();
        }
    }
}