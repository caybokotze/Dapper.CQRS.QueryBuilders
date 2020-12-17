using System;
using System.Data;
using Dapper;

namespace DapperDiddle
{
    public interface IBaseSqlExecutor
    {
        T SelectQuery<T>(string sql, object parameters);
        IDbConnection ReturnConnectionInstance();
        int Execute(string sql, object parameters);
    }
    
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