using DapperDoodle;

namespace TestProject.Commands
{
    public class SeedPeopleTable : Command
    {
        public override void Execute()
        {
            Execute("CREATE TABLE IF NOT EXISTS people (id INTEGER PRIMARY KEY, name TEXT NOT NULL, surname TEXT NOT NULL);");
        }
    }
}