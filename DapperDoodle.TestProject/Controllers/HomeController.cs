using System;
using System.Data.Common;
using System.Linq;
using DapperDoodle;
using DapperDoodle.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using PeanutButter.Utils;
using TestProject.Commands;
using TestProject.Queries;

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
            QueryExecutor.Execute(new SelectAPerson(person.Id));
            QueryExecutor.Execute(new TestBaseExecutor());
            return Content("All CRUD Operations Completed Successfully.");
        }
    }
}