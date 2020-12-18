using System;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using Dapper;
using DapperDiddle.Interfaces;
using MySql.Data.MySqlClient;

namespace DapperDiddle
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
            return (T)_connection.Query(sql, parameters);
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