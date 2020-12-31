using Dapper;
using DapperDoodle;

namespace TestProject.Queries
{
    public class TestBaseExecutor : Query<int>
    {
        public override void Execute()
        {
            var connectionInstance = GetIDbConnection();
            //
            var result = connectionInstance.QueryFirst("SELECT 1;");

            Result = result;
        }
    }
}