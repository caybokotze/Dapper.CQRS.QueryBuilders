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
            CommandExecutor.Execute(new TestExecutor());
            // CommandExecutor.Execute(new SeedPeopleTable());
            // var personId = CommandExecutor.Execute(new InsertAPerson());
            // CommandExecutor.Execute(new UpdateAPerson(personId));
            // QueryExecutor.Execute(new SelectAPerson(personId));
            return Content("Saved Successfully");
        }
    }

    public class InsertAPerson : Command<int>
    {
        public override void Execute()
        {
            Result = BuildInsert<Person>(new Person()
            {
                Id = GetRandomInt(),
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

    public class TestExecutor : Command<int>
    {
        public override void Execute()
        {
            var something = GetConnectionInstance().QueryFirst<int>("INSERT INTO People (name, surname) VALUES ('John', 'Williams'); SELECT last_insert_rowid();");
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