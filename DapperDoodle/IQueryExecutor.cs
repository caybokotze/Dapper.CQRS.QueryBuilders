using System.Collections.Generic;

namespace DapperDoodle
{
    public interface IQueryExecutor
    {
        T Execute<T>(Query<T> query);
    }
}