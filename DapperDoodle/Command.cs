using System;
using Google.Protobuf.WellKnownTypes;

namespace DapperDoodle
{
    public interface ICommand
    {
        void Execute();
    }
    
    public abstract class Command<T> : Command
    {
        public T Result { get; set; }
    }
    
    public abstract class Command : BaseSqlExecutor, ICommand
    {
        public abstract void Execute();

        public int BuildInsert<T>(object parameters)
        {
            return SelectQuery<int>(this.BuildInsertStatement<T>(), parameters: parameters);
        }

        public int BuildUpdate<T>(object parameters)
        {
            return SelectQuery<int>(this.BuildUpdateStatement<T>(), parameters: parameters);
        }

        /// <summary>
        /// This will automatically append the last inserted id for the record inserted.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        /// <exception cref="InvalidSqlStatementException"></exception>
        public int InsertAndReturnId(string sql, object parameters = null)
        {
            if(sql is null)
                throw new InvalidSqlStatementException();

            this.AppendReturnId(sql);
            
            return SelectQuery<int>(sql, parameters);
        }
    }
}