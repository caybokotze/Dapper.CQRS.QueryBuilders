using System;
using DapperDoodle;
using DapperDoodle.Interfaces;
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
            CommandExecutor.Execute(new SaveSomething());
            return Content("Saved Successfully");
        }
    }

    public class SaveSomething : Command
    {
        public override void Execute()
        {
            Console.WriteLine(BuildInsert<Person>());
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public int IdNumber { get; set; }
    }
}