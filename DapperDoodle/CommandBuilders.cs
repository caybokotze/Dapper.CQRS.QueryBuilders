using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
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
        /// <param name="case"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string BuildInsertStatement<T>(this Command command, Case @case)
        {
            return BuildInsertStatement<T>(command, null, @case);
        }

        /// <summary>
        /// Returns a string of the Insert Statement that will be inserted into the database.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="table"></param>
        /// <param name="case"></param>
        /// <param name="removeParameters"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string BuildInsertStatement<T>(this Command command, string table, Case @case, object removeParameters = null)
        {
            var dt = typeof(T).DataTableForType();

            table ??= typeof(T).Name.Pluralize().ConvertCase(@case);

            
            var sqlStatement = new StringBuilder();

            if (removeParameters != null)
            {
                var type = removeParameters.GetType();
                var props = new List<PropertyInfo>(type.GetProperties());

                foreach (var prop in props)
                {
                    dt.Columns.Remove(prop.Name);
                }
            }
            
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
                sqlStatement.Append($"{column.ColumnName.ConvertCase(@case)}, ");
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
        
        
        public static string BuildUpdateStatement<T>(this Command command)
        {
            return BuildUpdateStatement<T>(command, null, Case.Lowercase, null);
        }
        
        public static string BuildUpdateStatement<T>(this Command command, string table, string clause)
        {
            return BuildUpdateStatement<T>(command, table, Case.Lowercase, clause);
        }

        /// <summary>
        /// Returns a SQL UPDATE statement with the override for a where clause to specify the where condition.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="case"></param>
        /// <param name="clause"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string BuildUpdateStatement<T>(this Command command, Case @case, string clause)
        {
            return BuildUpdateStatement<T>(command, null, @case, clause);
        }
        
        /// <summary>
        /// Returns a SQL UPDATE statement with the override for a where clause and an override for the case and table name.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="table"></param>
        /// <param name="case"></param>
        /// <param name="clause"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="InvalidDatabaseTypeException"></exception>
        public static string BuildUpdateStatement<T>(this Command command, string table, Case @case, string clause)
        {
            var dt = typeof(T).DataTableForType();

            if (table is null)
                table = typeof(T).Name.Pluralize().ConvertCase(@case);
            
            if (clause is null)
                clause = "WHERE id = @Id;";

            var sqlStatement = new StringBuilder();

            switch (command.Dbms)
            {
                case DBMS.SQLite:
                    sqlStatement.Append($"UPDATE {table} SET");
                    break;
                case DBMS.MySQL:
                    sqlStatement.Append($"UPDATE `{table}` SET");
                    break;
                case DBMS.MSSQL:
                    sqlStatement.Append($"UPDATE [{table}] SET");
                    break;
                default:
                    throw new InvalidDatabaseTypeException();
            }

            sqlStatement.Append(" ");
            
            foreach (DataColumn column in dt.Columns)
            {
                sqlStatement.Append($"{column.ColumnName.ConvertCase(@case)} = @{column.ColumnName}, ");
            }

            sqlStatement.Remove(sqlStatement.Length - 2, 2);

            sqlStatement.Append(" ");

            sqlStatement.Append(clause);

            sqlStatement.Append(" SELECT 0;");
            
            return sqlStatement.ToString();
        }


        public static string BuildDeleteStatement<T>(this Command command)
        {
            return BuildDeleteStatement<T>(command, null, Case.Lowercase, null);
        }
        
        /// <summary>
        /// Returns a SQL DELETE statement.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="table"></param>
        /// <param name="casing"></param>
        /// <param name="clause"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="InvalidDatabaseTypeException"></exception>
        public static string BuildDeleteStatement<T>(this Command command, string table, Case @case, string clause)
        {
            if (table is null)
                table = typeof(T).Name.Pluralize().ConvertCase(@case);
                
            if (clause is null)
                clause = "WHERE id = @Id";
            
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
            sqlStatement.Append(clause);

            return sqlStatement.ToString();
        }
    }
}