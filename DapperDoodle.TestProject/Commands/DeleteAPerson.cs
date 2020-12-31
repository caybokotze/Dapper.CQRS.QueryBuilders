using DapperDoodle;
using PeanutButter.RandomGenerators;
using TestProject.Models;

namespace TestProject.Commands
{
    public class DeleteAPerson : Command
    {
        public override void Execute()
        {
            BuildDelete<Person>(new Person()
            {
                Name = RandomValueGen.GetRandomString(),
                Surname = RandomValueGen.GetRandomString()
            });
        }
    }
}