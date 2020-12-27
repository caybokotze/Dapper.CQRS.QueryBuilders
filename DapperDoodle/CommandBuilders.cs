using System;
using System.Data;
using System.Text;

namespace DapperDoodle
{
    public static class CommandBuilders
    {
        public static string AppendReturnInsertedId()
        {
            return "";
        }
        
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
                    throw new ArgumentException("The DBMS Selected is not yet supported");
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

        public static string BuildUpdateStatement<T>(this Command command)
        {
            return "";
        }

        public static string BuildSelectStatement<T>(this Command command)
        {
            return "";
        }
    }
}