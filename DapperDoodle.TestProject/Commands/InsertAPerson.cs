using DapperDoodle;
using PeanutButter.RandomGenerators;
using TestProject.Models;

namespace TestProject.Commands
{
    public class InsertAPerson : Command<Person>
    {
        public override void Execute()
        {
            var person = new Person()
            {
                Name = RandomValueGen.GetRandomString(),
                Surname = RandomValueGen.GetRandomString()
            };
            
            person.Id = BuildInsert<Person>(person, null, Case.Lowercase, new { Id = 0 });
            
            Result = person;
        }
    }
}