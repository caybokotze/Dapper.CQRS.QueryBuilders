using System;
using System.Transactions;
using Microsoft.Extensions.DependencyInjection;
using NExpect;
using NSubstitute;
using NUnit.Framework;
using PeanutButter.RandomGenerators;
using TestRunner;
using static NExpect.Expectations;
using static PeanutButter.RandomGenerators.RandomValueGen;

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

                    CommandExecutor.Execute(new InsertPerson(GetRandomPerson()));
                    
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

        public class Person
        {
            public string Name { get; set; }
            public string Surname { get; set; }
        }

        public static Person GetRandomPerson()
        {
            return new Person()
            {
                Name = GetRandomString(),
                Surname = GetRandomString()
            };
        }
        
        public static Command Create()
        {
            return new RandomCommand();
        }
    }
}