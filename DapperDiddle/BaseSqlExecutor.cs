using System;
using System.Data;
using Dapper;
using DapperDiddle.Interfaces;

namespace DapperDiddle
{
    public class BaseSqlExecutor : IBaseSqlExecutor
    {
        public BaseSqlExecutor()
        {
            
        }

        public BaseSqlExecutor(IDbConnection connection)
        {
            Connection = connection;
        }
        
        private IDbConnection Connection { get; }

        public T SelectQuery<T>(string sql, object parameters = null)
        {
            return (T)Connection.Query(sql, parameters);
        }

        public IDbConnection ReturnConnectionInstance()
        {
            return Connection;
        }

        public int Execute(string sql, object parameters = null)
        {
            return Connection.Execute(sql, parameters);
        }
    }
}