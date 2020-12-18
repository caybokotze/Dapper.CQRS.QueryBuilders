using System;
using System.Collections.Generic;
using DapperDoodle.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DapperDoodle
{
    public class QueryExecutor : IQueryExecutor
    {
        public IBaseSqlExecutorOptions Options { get; }

        public QueryExecutor(IServiceProvider services)
        {
            Options = services.GetService<IBaseSqlExecutorOptions>();
        }
        
        public void Execute(Query query)
        {
            query.InitialiseDependencies(Options);
            ExecuteWithNoResult(query);
        }

        private void ExecuteWithNoResult(Query query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            query.QueryExecutor = this;
            query.Execute();
        }

        public T Execute<T>(Query<T> query)
        {
            ExecuteWithNoResult(query);
            return query.Result;
        }

        public void Execute(IEnumerable<Query> queries)
        {
            if (queries == null)
            {
                throw new ArgumentNullException(nameof (queries));
            }

            foreach (Query query in queries)
            {
                Execute(query);
            }
        }
    }
}