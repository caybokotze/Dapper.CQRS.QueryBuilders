using DapperDoodle.Interfaces;

namespace DapperDoodle
{
    public abstract class Query<T> : Query
    {
        public T Result { get; set; }
    }
    
    public abstract class Query : BaseSqlExecutor
    {
        public IQueryExecutor QueryExecutor { get; set; }

        public abstract void Execute();

        public T BuildSelect<T>()
        {
            return SelectQuery<T>(this.BuildSelectStatement<T>());
        }
    }
}