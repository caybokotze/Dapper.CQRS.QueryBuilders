using System;
using System.Data.Common;
using System.Linq;
using Dapper;
using DapperDoodle;
using Microsoft.AspNetCore.Mvc;
using PeanutButter.Utils;
using static PeanutButter.RandomGenerators.RandomValueGen;

namespace TestProject.Controllers
{
    [Route("")]
    [Controller]
    public class HomeController : Controller
    {
        public ICommandExecutor CommandExecutor { get; }
        public IQueryExecutor QueryExecutor { get; }

        public HomeController(
            ICommandExecutor commandExecutor, 
            IQueryExecutor queryExecutor)
        {
            CommandExecutor = commandExecutor;
            QueryExecutor = queryExecutor;
        }
        
        [Route("")]
        public ActionResult Index()
        {
            CommandExecutor.Execute(new SeedPeopleTable());
            var person = CommandExecutor.Execute(new InsertAPerson());
            CommandExecutor.Execute(new UpdateAPerson(person));
            var samePerson = QueryExecutor.Execute(new SelectAPerson(person.Id));
            return Content("Saved Successfully");
        }
        
        [Route("test")]
        public ActionResult TestExecutor()
        {
            QueryExecutor.Execute(new TestExecutor());
            return Content("Saved Successfully");
        }
    }

    public class InsertAPerson : Command<Person>
    {
        public override void Execute()
        {
            var person = new Person()
            {
                Name = GetRandomString(),
                Surname = GetRandomString()
            };
            
            person.Id = BuildInsert<Person>(person, null, Case.Lowercase, new { Id = 0 });
            
            Result = person;
        }
    }

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
                Name = GetRandomString(),
                Surname = GetRandomString()
            });
        }
    }

    public class DeleteAPerson : Command
    {
        public override void Execute()
        {
            BuildDelete<Person>(new Person()
            {
                Name = GetRandomString(),
                Surname = GetRandomString()
            });
        }
    }
    
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

    public class TestExecutor : Query<int>
    {
        public override void Execute()
        {
            var something = GetConnectionInstance()
                .Query<int>("SELECT 1;").First();

            Result = something;
            
            var somethingType = something.GetType();
        }
    }

    public class SeedPeopleTable : Command
    {
        public override void Execute()
        {
            Execute("CREATE TABLE IF NOT EXISTS people (id INTEGER PRIMARY KEY, name TEXT NOT NULL, surname TEXT NOT NULL);");
        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}