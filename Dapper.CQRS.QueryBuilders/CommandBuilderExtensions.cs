namespace Dapper.CQRS.QueryBuilders
{
    public static class CommandBuilderExtensions
    {
        public static int BuildInsert<T>(this Command command, object parameters)
        {
            return command.QueryFirst<int>(command.BuildInsertStatement<T>(), parameters);
        }

        public static int BuildInsert<T>(this Command command, object parameters, string table)
        {
            return command.QueryFirst<int>(command.BuildInsertStatement<T>(table), parameters);
        }

        public static int BuildInsert<T>(this Command command, object parameters, string table, Case @case)
        {
            return command.QueryFirst<int>(command.BuildInsertStatement<T>(table, @case), parameters);
        }

        public static int BuildInsert<T>(this Command command, object parameters, string table, Case @case, object removeParameters)
        {
            return command.QueryFirst<int>(command.BuildInsertStatement<T>(table, @case, removeParameters), parameters);
        }

        public static int BuildUpdate<T>(this Command command, object parameters, string clause)
        {
            return command.QueryFirst<int>(command.BuildUpdateStatement<T>(null, clause), parameters);
        }

        public static int BuildUpdate<T>(this Command command, object parameters, string table, string clause)
        {
            return command.QueryFirst<int>(command.BuildUpdateStatement<T>(table, clause), parameters);
        }

        public static int BuildUpdate<T>(this Command command, object parameters)
        {
            return command.QueryFirst<int>(command.BuildUpdateStatement<T>(), parameters);
        }

        public static int BuildDelete<T>(this Command command, object parameters)
        {
            return command.QueryFirst<int>(command.BuildDeleteStatement<T>(), parameters);
        }
        
        public static int InsertAndReturnId(this Command command, string sql, object parameters = null)
        {
            if (sql is null)
            {
                throw new InvalidSqlStatementException();
            }

            return command.QueryFirst<int>(sql, parameters);
        }
    }
}