using System;
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

        public T SelectQuery<T>(string sql, object parameters = null)
        {
            if (typeof(T) == typeof(int))
            {
                return _connection.QueryFirst<T>(sql, parameters);
            }
            
            return (T)_connection.Query<T>(sql, parameters);
        }

        public IDbConnection GetConnectionInstance()
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