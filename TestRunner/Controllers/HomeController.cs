using System;
using DapperDiddle;
using DapperDiddle.Commands;
using DapperDiddle.Interfaces;
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
            ICommandExecutor commandExecutor)
        {
            CommandExecutor = commandExecutor;
        }
        
        [Route("")]
        public ActionResult Index()
        {
            CommandExecutor.Execute(new SaveSomething());
            //
            return Content("Hello there good sir.");
        }
    }

    public class SaveSomething : Command
    {
        public override void Execute()
        {
            Console.WriteLine(GetConnectionInstance().ConnectionString);
        }
    }
}