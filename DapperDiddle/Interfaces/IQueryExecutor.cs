using System.Collections.Generic;
using DapperDiddle.Queries;

namespace DapperDiddle.Interfaces
{
    public interface IQueryExecutor
    {
        void Execute(Query query);
        T Execute<T>(Query<T> query);
        void Execute(IEnumerable<Query> queries);
    }
}