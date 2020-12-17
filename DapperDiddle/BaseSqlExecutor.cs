using System.Data;
using Dapper;

namespace DapperDiddle
{
    public class BaseSqlExecutor
    {
        private IDbConnection Connection { get; }

        public BaseSqlExecutor(IDbConnection connection)
        {
            Connection = connection;
        }

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