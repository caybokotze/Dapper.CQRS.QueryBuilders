using System;
using DapperDoodle;
using Microsoft.AspNetCore.Mvc;

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
            CommandExecutor.Execute(new InsertAPerson());
            return Content("Saved Successfully");
        }
    }

    public class InsertAPerson : Command
    {
        public override void Execute()
        {
            BuildInsert<Person>(new Person()
            {
                Name = "Honey",
                Surname = "Maxwell"
            });
        }
    }

    public class UpdateAPerson : Command
    {
        public override void Execute()
        {
            BuildUpdate<Person>(new Person()
            {
                Name = "Honey",
                Surname = "Maxwell"
            });
        }
    }

    public class DeleteAPerson : Command
    {
        public override void Execute()
        {
            BuildDelete<Person>(new Person()
            {
                
            });
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}