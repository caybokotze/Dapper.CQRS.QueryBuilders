using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using DapperDoodle.Interfaces;

namespace DapperDoodle
{
    public class BaseSqlExecutor
    {
        public void InitialiseDependencies(IBaseSqlExecutorOptions options)
        {
            _connection = options.Connection;
            Dbms = options.Dbms;
        }

        private IDbConnection _connection;
        public DBMS Dbms { get; set; }

        protected T QueryFirst<T>(string sql, object parameters = null)
        {
            return (T)_connection.Query<T>(sql, parameters);
        }
        
        protected List<T> QueryList<T>(string sql, object parameters = null)
        {
            return (List<T>)_connection.Query<T>(sql, parameters);
        }

        public IDbConnection GetIDbConnection()
        {
            return _connection;
        }

        protected int Execute(string sql, object parameters = null)
        {
            if (sql.Equals("", StringComparison.InvariantCulture) || sql is null)
                throw new ArgumentException("Please specify a value for the sql attribute.");
            
            return _connection.Execute(sql, parameters);
        }
    }
}