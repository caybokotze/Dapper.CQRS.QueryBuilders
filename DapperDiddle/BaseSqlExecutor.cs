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
            _connection = new MySqlConnection(options.ConnectionString);
            _database = options.Database;
        }

        private IDbConnection _connection;
        private Dbms _database;

        public T SelectQuery<T>(string sql, object parameters = null)
        {
            return (T)_connection.Query(sql, parameters);
        }

        public IDbConnection ReturnConnectionInstance()
        {
            return _connection;
        }

        public string ReturnConnectionString()
        {
            return _connection.ConnectionString;
        }

        public int Execute(string sql, object parameters = null)
        {
            return _connection.Execute(sql, parameters);
        }
    }
}