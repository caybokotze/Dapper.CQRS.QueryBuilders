using System.Transactions;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using MySqlX.XDevAPI.Common;
using NExpect;
using NUnit.Framework;
using TestRunner;
using Ubiety.Dns.Core;
using static NExpect.Expectations;

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
            private readonly IServiceCollection _dependencies;
            public ICommandExecutor CommandExecutor { get; set; }
            public IQueryExecutor QueryExecutor { get; set; }

            [SetUp]
            public void Setup()
            {
                var serviceProvider = ServiceProviderFactory.ServiceProvider;
                
                CommandExecutor = serviceProvider.GetService<ICommandExecutor>();
                QueryExecutor = serviceProvider.GetService<IQueryExecutor>();
            }
            
            [Test]
            public void ShouldInsertRecordForMySql()
            {
                using (var scope = new TransactionScope())
                {
                    var command = Create();
                    command.Dbms = DBMS.MySQL;

                    var person = new Person()
                    {
                        Name = "John",
                        Surname = "Simmons"
                    };

                    command.BuildInsert<Person>(person);
                    
                    var commandExecutor = new CommandExecutor();
                    scope.Complete();
                }
            }
        }

        public class RandomCommand : Command
        {
            public override void Execute()
            {
                
            }
        }

        public class Person
        {
            public string Name { get; set; }
            public string Surname { get; set; }
        }
        
        public static Command Create()
        {
            return new RandomCommand();
        }
    }
}