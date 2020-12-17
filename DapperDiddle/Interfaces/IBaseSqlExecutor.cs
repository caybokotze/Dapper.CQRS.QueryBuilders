using System.Data;

namespace DapperDiddle.Interfaces
{
    public interface IBaseSqlExecutor
    {
        T SelectQuery<T>(string sql, object parameters);
        IDbConnection ReturnConnectionInstance();
        int Execute(string sql, object parameters);
    }
}