﻿using System;
using System.Transactions;
using Dapper.CQRS.QueryBuilder.Tests.TestModels;
using Dapper.CQRS.QueryBuilders;
using NExpect;
using NUnit.Framework;
using static NExpect.Expectations;

namespace Dapper.CQRS.QueryBuilder.Tests
{
    [TestFixture]
    public class CommandTests
    {
        private IServiceProvider _serviceProvider;
        private ICommandExecutor _commandExecutor;

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
                var expected = @"INSERT INTO `people` (id, name, surname, email) VALUES (@Id, @Name, @Surname, @Email); SELECT LAST_INSERT_ID();";
                
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
                var expected = @"INSERT INTO people (id, name, surname, email) VALUES (@Id, @Name, @Surname, @Email); SELECT last_insert_rowid();";
                
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
                var expected = @"INSERT INTO [people] (id, name, surname, email) VALUES (@Id, @Name, @Surname, @Email); SELECT SCOPE_IDENTITY();";
                
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
                    // todo: complete...
                }
            }
            
            [Test]
            public void ShouldUpdateRecordForMySql()
            {
                using (var scope = new TransactionScope())
                {
                    // todo: complete...
                }
            }
            
            [Test]
            public void ShouldDeleteRecordForMySql()
            {
                using (var scope = new TransactionScope())
                {
                    // todo: complete...
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
                this.BuildInsert<Person>(_person);
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