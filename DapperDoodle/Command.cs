using System;
using DapperDoodle.Exceptions;
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
        

        /// <summary>
        /// Returns the ID of the Inserted record after inserting the record.
        /// </summary>
        /// <param name="parameters">Pass in the arguments or type as an argument that needs to be appended to the sql statement</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public int BuildInsert<T>(object parameters)
        {
            return SelectQuery<int>(this.BuildInsertStatement<T>(), parameters: parameters);
        }
        
        public int BuildInsert<T>(object parameters, string table)
        {
            return SelectQuery<int>(this.BuildInsertStatement<T>(table: table), parameters: parameters);
        }
        
        public int BuildInsert<T>(object parameters, string table, Case casing)
        {
            return SelectQuery<int>(this.BuildInsertStatement<T>(table: table, casing: casing), parameters: parameters);
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