﻿using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace DapperDiddle.Commands
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

        public void BuildInsert<T>()
        {
            Execute(this.BuildInsertStatement<T>(), typeof(T));
        }

        public void BuildInsert<T>(object parameters)
        {
            Execute(this.BuildInsertStatement<T>(), parameters);
        }

        /// <summary>
        /// This will execute an instance of a Command. The Insert Statement is automatically generated from the type that is passed into the method.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        /// <exception cref="InvalidSqlStatementException"></exception>
        public int BuildInsert(string sql, object parameters = null)
        {
            if(sql is null)
                throw new InvalidSqlStatementException();
            
            StringBuilder builder = new StringBuilder(sql);
            builder.Append("");
            
            return Execute(sql, parameters);
        }
    }
}