namespace Dapper.CQRS
{
    public interface IQueryExecutor
    {
        T Execute<T>(Query<T> query);
    }
}