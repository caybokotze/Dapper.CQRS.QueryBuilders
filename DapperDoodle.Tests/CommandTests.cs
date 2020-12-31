using System.Transactions;
using DapperDoodle.Tests.TestModels;
using Microsoft.Extensions.DependencyInjection;
using NExpect;
using NUnit.Framework;
using PeanutButter.Utils;
using TestProject;
using static NExpect.Expectations;
using static PeanutButter.RandomGenerators.RandomValueGen;
using ServiceProvider = TestProject.ServiceProvider;

namespace DapperDoodle.Tests
{
    [TestFixture]
    public class CommandTests
    {
        [TestFixture]
        public class Behaviour
        {
            [Test]
            public void ShouldBuildInsertForMySql()
            {
                // Arrange
                var command = Create();
                command.Dbms = DBMS.MySQL;

                // Act
                var insertStatement = command.BuildInsertStatement<Person>();
                var expected = @"INSERT INTO `people` (name, surname) VALUES (@Name, @Surname); SELECT LAST_INSERT_ID();";
                
                // Assert
                Expect(insertStatement).To.Equal(expected);
            }
            
            [Test]
            public void ShouldBuildInsertForSqlLite()
            {
                // Arrange
                var command = Create();
                command.Dbms = DBMS.SQLite;

                // Act
                var insertStatement = command.BuildInsertStatement<Person>();
                var expected = @"INSERT INTO people (name, surname) VALUES (@Name, @Surname); SELECT last_insert_rowid();";
                
                // Assert
                Expect(insertStatement).To.Equal(expected);
            }
            
            [Test]
            public void ShouldBuildInsertForMsSql()
            {
                // Arrange
                var command = Create();
                command.Dbms = DBMS.MSSQL;

                // Act
                var insertStatement = command.BuildInsertStatement<Person>();
                var expected = @"INSERT INTO [people] (name, surname) VALUES (@Name, @Surname); SELECT SCOPE_IDENTITY();";
                
                // Assert
                Expect(insertStatement).To.Equal(expected);
            }
        }

        public class Transactions
        {
            public ICommandExecutor CommandExecutor { get; set; }
            public IQueryExecutor QueryExecutor { get; set; }

            [SetUp]
            public void Setup()
            {
                ServiceProvider.CreateHost();
                CommandExecutor = ServiceActivator.GetScope().ServiceProvider.GetService<ICommandExecutor>();
                QueryExecutor = ServiceActivator.GetScope().ServiceProvider.GetService<IQueryExecutor>();
            }
            
            [Test]
            public void ShouldInsertRecordForMySql()
            {
                using (var scope = new TransactionScope())
                {
                    var command = Create();
                    command.Dbms = DBMS.MySQL;

                    var person = Person.Create();

                    command.BuildInsert<Person>(person);

                    CommandExecutor.Execute(new InsertPerson(person));
                    
                    scope.Complete();
                }
            }
        }

        public class InsertPerson : Command
        {
            private readonly Person _person;

            public InsertPerson(Person person)
            {
                _person = person;
            }
            
            public override void Execute()
            {
                BuildInsert<Person>(_person);
            }
        }

        public class RandomCommand : Command
        {
            public override void Execute()
            {
                
            }
        }

        public static Command Create()
        {
            return new RandomCommand();
        }
    }
}