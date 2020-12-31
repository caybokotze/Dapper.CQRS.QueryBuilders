using DapperDoodle;
using PeanutButter.RandomGenerators;
using TestProject.Models;

namespace TestProject.Commands
{
    public class UpdateAPerson : Command
    {
        public Person Person { get; }

        public UpdateAPerson(Person person)
        {
            Person = person;
        }
        public override void Execute()
        {
            BuildUpdate<Person>(new Person()
            {
                Id = Person.Id,
                Name = RandomValueGen.GetRandomString(),
                Surname = RandomValueGen.GetRandomString()
            });
        }
    }
}