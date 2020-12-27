using System.Transactions;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using MySqlX.XDevAPI.Common;
using NExpect;
using NUnit.Framework;
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
            public void ShouldInsertRecord()
            {
                using (var scope = new TransactionScope())
                {
                    scope.Complete();
                }
            }

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