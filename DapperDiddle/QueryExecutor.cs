using System;
using System.Collections;
using System.Collections.Generic;
using DapperDiddle.Interfaces;

namespace DapperDiddle
{
    public class QueryExecutor : IQueryExecutor
    {
        public void Execute(Query query)
        {
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