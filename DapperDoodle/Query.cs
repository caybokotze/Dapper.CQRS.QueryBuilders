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
        
        public T BuildSelect<T>(Case @case)
        {
            return SelectQuery<T>(this.BuildSelectStatement<T>(@case));
        }

        public T BuildSelect<T>(string clause, object parameters = null)
        {
            return SelectQuery<T>(this.BuildSelectStatement<T>(null, clause));
        }
        
        public T BuildSelect<T>(string table, string clause, object parameters = null)
        {
            return SelectQuery<T>(this.BuildSelectStatement<T>(table, clause), parameters: parameters);
        }
    }
}