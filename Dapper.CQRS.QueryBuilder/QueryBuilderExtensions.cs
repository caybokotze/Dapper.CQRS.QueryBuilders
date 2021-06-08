using System.Collections.Generic;

namespace Dapper.CQRS.QueryBuilder
{
    public static class QueryBuilderExtensions
    {
        public static List<T> BuildSelect<T>(this Query query, object parameters = null)
        {
            return query.QueryList<T>(query.BuildSelectStatement<T>(), parameters);
        }
        
        public static List<T> BuildSelect<T>(this Query query, Case @case)
        {
            return query.QueryList<T>(query.BuildSelectStatement<T>(@case));
        }

        public static List<T> BuildSelect<T>(this Query query, string clause, object parameters = null)
        {
            return query.QueryList<T>(query.BuildSelectStatement<T>(null, clause), parameters);
        }
        
        public static List<T> BuildSelect<T>(this Query query, string table, string clause, object parameters = null)
        {
            return query.QueryList<T>(query.BuildSelectStatement<T>(table, clause), parameters);
        }
        
        public static List<T> BuildSelect<T>(this Query query, string table, string clause, Case @case, object parameters = null, object ignoreParameters = null)
        {
            return query.QueryList<T>(query.BuildSelectStatement<T>(table, @case, clause, ignoreParameters), parameters);
        }
    }
}