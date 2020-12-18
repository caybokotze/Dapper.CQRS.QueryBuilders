using System;
using System.Data;
using System.Runtime.CompilerServices;
using Dapper;
using DapperDiddle.Enums;
using DapperDiddle.Interfaces;
using MySql.Data.MySqlClient;

namespace DapperDiddle
{
    public class BaseSqlExecutor
    {
        public void InitialiseDependencies(IBaseSqlExecutorOptions options)
        {
            _connection = options.Connection;
            _dbms = options.Dbms;
        }

        private IDbConnection _connection;
        private DBMS _dbms;

        public T SelectQuery<T>(string sql, object parameters = null)
        {
            return (T)_connection.Query(sql, parameters);
        }

        public IDbConnection GetConnectionInstance()
        {
            return _connection;
        }

        public int Execute(string sql, object parameters = null)
        {
            return _connection.Execute(sql, parameters);
        }
    }
}