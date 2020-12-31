using System;
using System.Threading.Tasks;
using System.Transactions;
using DapperDoodle.Tests.TestModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NExpect;
using NSubstitute.Extensions;
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
        private IServiceProvider _serviceProvider;
        private ICommandExecutor _commandExecutor;
        
        public void Init()
        {
            var services = new ServiceCollection();
            services.AddOptions();
            services.ConfigureDapperDoodle(null, DBMS.SQLite);
            _serviceProvider = services.BuildServiceProvider();
            _commandExecutor = _serviceProvider.GetService<ICommandExecutor>();
        }

        [Test]
        public void TestOne()
        {
            var command = _commandExecutor;
        }
        
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

        [TestFixture]
        public class Transactions
        {
            [Test]
            public void ShouldInsertRecordForMySql()
            {
                using (var scope = new TransactionScope())
                {
                    var command = Create();
                    command.Dbms = DBMS.MySQL;

                    var person = Person.Create();

                    command.BuildInsert<Person>(person);

                    //_commandExecutor.Execute(new InsertPerson(person));
                    
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