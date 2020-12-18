using System.Data;
using DapperDiddle.Enums;

namespace DapperDiddle.Interfaces
{
    public interface IBaseSqlExecutorOptions
    {
        string ConnectionString { get; set; }
        Dbms Database { get; set; }
    }

    public class BaseSqlExecutorOptions : IBaseSqlExecutorOptions
    {
        public string ConnectionString { get; set; }
        public Dbms Database { get; set; }
    }
}