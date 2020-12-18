using System.Collections.Generic;

namespace DapperDoodle.Interfaces
{
    public interface IQueryExecutor
    {
        void Execute(Query query);
        T Execute<T>(Query<T> query);
        void Execute(IEnumerable<Query> queries);
    }
}