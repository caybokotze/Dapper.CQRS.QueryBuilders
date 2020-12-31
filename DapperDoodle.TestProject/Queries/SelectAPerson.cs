using DapperDoodle;
using TestProject.Models;

namespace TestProject.Queries
{
    public class SelectAPerson : Query<Person>
    {
        private readonly int _id;

        public SelectAPerson(int Id)
        {
            _id = Id;
        }
        public override void Execute()
        {
            Result = BuildSelect<Person>("where id = @Id", new { Id = _id });
        }
    }
}