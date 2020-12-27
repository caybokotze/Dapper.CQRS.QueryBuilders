using System;
using DapperDoodle;
using Microsoft.AspNetCore.Mvc;
using PeanutButter.RandomGenerators;
using static PeanutButter.RandomGenerators.RandomValueGen;

namespace TestRunner.Controllers
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
            var personId = CommandExecutor.Execute(new InsertAPerson());
            CommandExecutor.Execute(new UpdateAPerson(personId));
            QueryExecutor.Execute(new SelectAPerson(personId));
            return Content("Saved Successfully");
        }
    }

    public class InsertAPerson : Command<int>
    {
        public override void Execute()
        {
            Result = BuildInsert<Person>(new Person()
            {
                Name = GetRandomString(),
                Surname = GetRandomString()
            });
        }
    }

    public class UpdateAPerson : Command
    {
        public int Id { get; }

        public UpdateAPerson(int id)
        {
            Id = id;
        }
        public override void Execute()
        {
            BuildUpdate<Person>(new Person()
            {
                Id = Id,
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

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}